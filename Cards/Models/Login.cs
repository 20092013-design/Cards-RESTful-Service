using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Cards.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
       
        public string Password { get; set; }
    }

    public class ResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
    public class CardModel
    {
        [Key]
        public int CardNo { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
        
        //[HexColor("Color has to have format '#123456'")]
        public string Color { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
    }
    public class CardOneModel
    {
       
        public int CardNo { get; set; }
      
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Status { get; set; }
        public string? CreatedBy { get; set; }
    }
    public static class userRoles
    {
        public const string Member = "Member";
        public const string Admin = "Admin";
    }
    public class StateViewModel<T>
    {
        public int Code { set; get; }
        public string Status { get; set; }
        public string Message { set; get; }
        public T Data { set; get; }
    }
    public class ConstantVal
    {
        public const int Success = 200;
        public const string SuccessMsg = "Success";
        public const int Error = 300;
        public const string ErrorMsg = "Failed";
        public const string NoRecordMsg = "No records found";
        public const string UnKnownError = "Request could not be proccessed. Unkown error";   
    
    }
    public class GeneralResultModel
    {
        public int? Code { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class CardViewResult
    {
        public string Message { get; set; }
        public int Code { get; set; }
    }
    public class GetOneCardmodel
    {
        public int CardNo { get; set; }
    }
    public class LoginToken
    {
        public string id { get; set; }
        public string Password { get; set; }
        public string? RoleID { get; set; }
    }

    public class TokenDetails
    {
        public string? Email { get; set; }        
        public string? RoleID { get; set; }
    }
    public class UserAuthDetails
    {
        public string Email { get; set; }
        public string RoleID { get; set; }
        public string Token { get; set; }
    
    }
    public class SearchCardModel
    {


        public string? SearchName { get; set; }
        public char? SearchType { get; set; }
    }
    public class SearchCardResultModel
    {

        public int CardNo { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
