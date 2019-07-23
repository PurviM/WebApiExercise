using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI_CRUD.Models;

namespace WebAPI_CRUD.Controllers
{
    public class MemberController : ApiController
    {
        //Create instance of Entity model 
        private testEntities txt = new testEntities();

        //This action method return all members records. 
        // GET api/<controller>
        public IEnumerable<tblmember> Get()
        {
            return txt.tblmembers.ToList().AsEnumerable();
           // return new string[] { "value1", "value2" };
        }

        //This action method will fetch and filter for specific member id record  
        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            //fetching and filter specific member id record   
            var memberdetail = (from a in txt.tblmembers where a.MemberID == id select a).FirstOrDefault();
           
            //checking fetched or not with the help of NULL or NOT. 
            if (memberdetail != null)
            {
                //sending response as status code OK with memberdetail entity.  
                return Request.CreateResponse(HttpStatusCode.OK, memberdetail);
            }
            else
            {
                  //sending response as error status code NOT FOUND with meaningful message.  
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Code or Member Not Found");  
            }  
        
       
        }

        //To add a new member record  
        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]tblmember _member)
        {
            try
            {
                //To add an new member record  
                txt.tblmembers.Add(_member);

                //Save the submitted record  
               txt.SaveChanges();

                //return response status as successfully created with member entity  
                var msg = Request.CreateResponse(HttpStatusCode.Created, _member);

                //Response message with requesturi for check purpose  
                msg.Headers.Location = new Uri(Request.RequestUri + _member.MemberID.ToString());

                return msg;  
            }
            catch (Exception ex)
            {
                //return response as bad request  with exception message.  
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);  
            }
        }

        //To update member record  
        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, [FromBody]tblmember _member)
        {
            //fetching and filter specific member id record   
            var memberdetail = (from a in txt.tblmembers where a.MemberID == id select a).FirstOrDefault();

            //checking fetched or not with the help of NULL or NOT.  
            if (memberdetail != null)
            {
                //set received _member object properties with memberdetail  
                memberdetail.MemberName = _member.MemberName;
                memberdetail.PhoneNumber = _member.PhoneNumber;
                //save set allocation.  
                txt.SaveChanges();

                //return response status as successfully updated with member entity  
                return Request.CreateResponse(HttpStatusCode.OK, memberdetail);
            }
            else
            {
                //return response error as NOT FOUND  with message.  
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Code or Member Not Found");
            }
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                //fetching and filter specific member id record   
                var _DeleteMember = (from a in txt.tblmembers where a.MemberID == id select a).FirstOrDefault();

                //checking fetched or not with the help of NULL or NOT.  
                if (_DeleteMember != null)
                {

                    txt.tblmembers.Remove(_DeleteMember);
                    txt.SaveChanges();

                    //return response status as successfully deleted with member id  
                    return Request.CreateResponse(HttpStatusCode.OK, id);
                }
                else
                {
                    //return response error as Not Found  with exception message.  
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Member Not Found or Invalid " + id.ToString());
                }
            }

            catch (Exception ex)
            {

                //return response error as bad request  with exception message.  
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }  
        }
    }
}