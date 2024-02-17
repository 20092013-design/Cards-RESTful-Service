using Cards.Data;
using Cards.Models;
using Dapper;
using Humanizer;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;


namespace Cards.Services
{

    public class LoginService
    {

        IDbConnection conn = DBClient.GetInstance();

        public StateViewModel<LoginToken> UserAuth(LoginModel model)
        {
            StateViewModel<LoginToken> _state = new StateViewModel<LoginToken>();
            try
            {
                var _value = conn.Query<LoginToken>("p_validateuser",
                    new
                    {
                        Email = model.Email

                    }, null, true, 0, commandType: CommandType.StoredProcedure).SingleOrDefault();

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
