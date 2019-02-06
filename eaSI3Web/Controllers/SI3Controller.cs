﻿using System;
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
            try
            {
                NormalizarHoras(model);

                SI3Service SI3Service = new SI3Service(username, password);

                Dictionary<string, Dictionary<DayOfWeek, int>> weekWork = new Dictionary<string, Dictionary<DayOfWeek, int>>();

                foreach (var dateIssue in model)
                {
                    Console.WriteLine("estoy dentro");
                    foreach (var issue in dateIssue.Issues)
                    {
                        int timeToInt = (int)issue.Tiempo; //SI3 no permite horas parciales, solo horas enteras.

                        int idNumber;
                        if (Int32.TryParse(issue.IssueSI3Code, out idNumber))
                        {
                            SI3Service.AddIssueWork(issue.IssueSI3Code, dateIssue.Fecha, timeToInt);
                        }
                        else
                        {
                            if (!weekWork.ContainsKey(issue.IssueSI3Code))
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

                foreach (var week in weekWork)
                {
                    SI3Service.AddProjectWork(week.Key, week.Value);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return string.Empty;
        }

        public void NormalizarHoras(IEnumerable<WeekJiraIssues> tareasSinNormalizar)
        {
            var diasConHorasDeMas = tareasSinNormalizar.Where(x => x.Issues.Sum(y => (y.Tiempo)) > 8);
            var diasConHorasDeMenos = tareasSinNormalizar.Where(x => x.Issues.Sum(y => (y.Tiempo)) < 8);
            Queue<WeekJiraIssues.JiraIssues> sobrante = new Queue<WeekJiraIssues.JiraIssues>();

            foreach (var diaConSobrante in diasConHorasDeMas)
            {
                double horas_sobrantes = diaConSobrante.Issues.Sum(x => x.Tiempo) - 8;

                var issuesConHorasSobrantes = diaConSobrante.Issues.Where(x => x.Tiempo >= (horas_sobrantes));

                if (issuesConHorasSobrantes != null && issuesConHorasSobrantes.Any())
                {
                    var issue = issuesConHorasSobrantes.First();

                    for (int i = 0; i < horas_sobrantes; i++)
                    {
                        var clonedIssue = (WeekJiraIssues.JiraIssues)issue.Clone();
                        clonedIssue.Tiempo = 1;
                        sobrante.Enqueue(clonedIssue);
                    }

                    issue.Tiempo -= horas_sobrantes;
                }
                else //Solo tiene issues de 1 hora
                {
                    var issues = diaConSobrante.Issues.Where(x => x.Tiempo == 1).ToList().Take((int)horas_sobrantes);
                    foreach (var issue in issues)
                    {
                        sobrante.Enqueue(issue);
                        diaConSobrante.Issues.Remove(issue);
                    }
                }
            }

            foreach (var diaConFaltante in diasConHorasDeMenos)
            {
                double horas_faltantes = Math.Abs(diaConFaltante.Issues.Sum(x => x.Tiempo) - 8);

                for (int i = 0; i < horas_faltantes; i++)
                {
                    var issue = sobrante.Dequeue();
                    diaConFaltante.Issues.Add(issue);
                }
            }
        }
    }
}
