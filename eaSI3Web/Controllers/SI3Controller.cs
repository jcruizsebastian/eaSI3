using System;
using System.Collections.Generic;
using System.Linq;
using SI3Connector;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static eaSI3Web.Controllers.JiraController;

namespace eaSI3Web.Controllers
{
    [Route("api/[controller]")]
    public class SI3Controller : Controller
    {
        [HttpPost("[action]")]
        public string Register([FromQuery]string username, [FromQuery]string password, [FromBody]IEnumerable<WeekJiraIssues> model)
        {
            SI3Service SI3Service = new SI3Service(username, password);

            //Recorrer el modelo y:
            //Si id de si3 que viene en la issue se puede convertir en un numero
            //Se trata de una imputación a issue SI3Service.AddIssueWork
            //sino
            //Se trata de una imputación a proyecto SI3Service.AddProjectWork
            Dictionary<string, Dictionary<DayOfWeek, int>> weekWork = new Dictionary<string, Dictionary<DayOfWeek, int>>();
            

            foreach (var dateIssue in model)
            {
                Console.WriteLine("estoy dentro");
                foreach (var issue in dateIssue.Issues)
                {
                    int timeToInt = (int)issue.Tiempo; //SI3 no permite horas parciales, solo horas enteras.

                    int idNumber;
                    if (Int32.TryParse(issue.IssueSI3Code,  out idNumber))
                    {
                        SI3Service.AddIssueWork( issue.IssueSI3Code, dateIssue.Fecha, timeToInt );
                    }
                    else
                    {
                        if(!weekWork.ContainsKey(issue.IssueSI3Code))
                        {
                            weekWork.Add(issue.IssueSI3Code, new Dictionary<DayOfWeek, int>());
                        }

                        Dictionary<DayOfWeek, int> dayWork = weekWork[issue.IssueSI3Code];

                        if (!dayWork.ContainsKey(dateIssue.Fecha.DayOfWeek))
                        {
                            dayWork.Add(dateIssue.Fecha.DayOfWeek, timeToInt);
                        }
                        else
                        {
                            dayWork[dateIssue.Fecha.DayOfWeek] += timeToInt;
                        }
                    }
                }
            }

            foreach(var week in weekWork)
            {
                SI3Service.AddProjectWork(week.Key, week.Value);
            }


            return "";
        }
    }
}
 