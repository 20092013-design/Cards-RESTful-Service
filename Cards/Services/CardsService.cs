using Cards.Data;
using Cards.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text.RegularExpressions;

namespace Cards.Services
{
    public class CardsService
    {
        IDbConnection conn = DBClient.GetInstance();
       
        public GeneralResultModel CreateCard(CardModel model, string email)

        {
            GeneralResultModel _state = new GeneralResultModel();
            try
            {
                var result = conn.Query<CardViewResult>("p_addCard",
             new
             {
                 Name = model.Name,
                 Description = model.Description,
                 Color = model.Color,
                 Status = model.Status,
                 CreatedBy = email

             }, null, true, 0, commandType: CommandType.StoredProcedure).SingleOrDefault();

                if (result != null)
                {
                    _state.Code = ConstantVal.Success;
                    _state.Status = ConstantVal.SuccessMsg;
                    _state.Message = result.Message;


                }
                else
                {
                    _state.Code = ConstantVal.Error;
                    _state.Status = ConstantVal.ErrorMsg;
                    _state.Message = ConstantVal.NoRecordMsg;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return _state;
        }


        public StateViewModel<List<CardOneModel>> GetAllcards(string role)
        {
            StateViewModel<List<CardOneModel>> _state = new StateViewModel<List<CardOneModel>>();

            try
            {
                var result = conn.Query<CardOneModel>("p_getallCards",
                     new
                     {
                         role =role

                     },
                     null, true, 0, commandType: CommandType.StoredProcedure).ToList();

                if (result != null)
                {
                    _state.Code = ConstantVal.Success;
                    _state.Status = ConstantVal.SuccessMsg;
                    _state.Message = ConstantVal.SuccessMsg;
                    _state.Data = result;
                }
                else
                {

                    _state.Code = ConstantVal.Error;
                    _state.Status = ConstantVal.ErrorMsg;
                    _state.Message = ConstantVal.NoRecordMsg;
                }
            }
            catch (Exception msg)
            {
                _state.Code = ConstantVal.Error;
                _state.Status = ConstantVal.ErrorMsg;
                _state.Message = ConstantVal.NoRecordMsg;
            }




            return _state;
        }



        public StateViewModel<List<CardModel>> GetOnecard(GetOneCardmodel model, string role,string email)
        {
            StateViewModel<List<CardModel>> _state = new StateViewModel<List<CardModel>>();

            try
            {
                var _value = conn.Query<CardModel>("p_getOneCard",
                    new
                    {
                        CardNo = model.CardNo,
                        role=role,
                        email=email
                    }, null, true, 0, commandType: CommandType.StoredProcedure).ToList();

                if (_value.Count!=0)
                {
                    _state.Code = ConstantVal.Success;
                    _state.Status = ConstantVal.SuccessMsg;
                    _state.Message = ConstantVal.SuccessMsg;
                    _state.Data = _value;
                }
                else
                {
                    _state.Code = ConstantVal.Error;
                    _state.Status = ConstantVal.ErrorMsg;
                    _state.Message = ConstantVal.NoRecordMsg;
                }
            }
            catch (Exception msg)
            {
               
                    _state.Code = ConstantVal.Error;
                    _state.Status = ConstantVal.ErrorMsg;
                    _state.Message = msg.Message;
                
            }


            return _state;

        }

        public GeneralResultModel DeleteCard(GetOneCardmodel model, string role,string email)

        {
            GeneralResultModel _state = new GeneralResultModel();
            try
            {
                var result = conn.Query<CardViewResult>("p_DeleteOneCard",
             new
             {
                 CardNo = model.CardNo,
                 role=role,
                 email=email

             }, null, true, 0, commandType: CommandType.StoredProcedure).SingleOrDefault();

                if (result != null && result.Code==200)
                {
                    _state.Code = ConstantVal.Success;
                    _state.Status = ConstantVal.SuccessMsg;
                    _state.Message = result.Message;


                }

                if (result != null && result.Code == 400)
                {
                    _state.Code = ConstantVal.Success;
                    _state.Status = ConstantVal.SuccessMsg;
                    _state.Message = result.Message;


                }
                else
                {
                    _state.Code = ConstantVal.Error;
                    _state.Status = ConstantVal.ErrorMsg;
                    _state.Message = ConstantVal.NoRecordMsg;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return _state;
        }
        public GeneralResultModel UpdateCard(CardOneModel model, string role,string email)

        {
            GeneralResultModel _state = new GeneralResultModel();
            try
            {
                var result = conn.Query<CardViewResult>("p_UpdateCard",
             new
             {
                 CardNo = model.CardNo,
                 Name=model.Name,
                 Description=model.Description,
                 Color=model.Color,
                 Status=model.Status,
                 role=role,
                 email=email

             }, null, true, 0, commandType: CommandType.StoredProcedure).SingleOrDefault();

                if (result != null)
                {
                    _state.Code = ConstantVal.Success;
                    _state.Status = ConstantVal.SuccessMsg;
                    _state.Message = result.Message;


                }
                else
                {
                    _state.Code = ConstantVal.Error;
                    _state.Status = ConstantVal.ErrorMsg;
                    _state.Message = ConstantVal.NoRecordMsg;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return _state;
        }

        public StateViewModel<List<T>> DetailsSearch<T>(SearchCardModel model,string role,string email)
        {
            StateViewModel<List<T>> _state = new StateViewModel<List<T>>();

            try
            {
                var searchParams = new
                {
                    SearchName = model.SearchName,
                    role=role,
                    email=email,
                    SearchType = model.SearchType
                };

                List<T> _value = conn.Query<T>("p_GetSearchResult", searchParams, null, true, 0, commandType: CommandType.StoredProcedure).ToList();

                if (_value != null)
                {
                    _state.Code = ConstantVal.Success;
                    _state.Status = ConstantVal.SuccessMsg;
                    _state.Message = ConstantVal.SuccessMsg;
                    _state.Data = _value;
                }
                else
                {
                    _state.Code = ConstantVal.Error;
                    _state.Status = ConstantVal.ErrorMsg;
                    _state.Message = ConstantVal.NoRecordMsg;
                }
            }
            catch (Exception msg)
            {
               
                    _state.Code = ConstantVal.Error;
                    _state.Status = ConstantVal.ErrorMsg;
                    _state.Message = msg.Message;
                
            }


            return _state;
        }

    }
}
