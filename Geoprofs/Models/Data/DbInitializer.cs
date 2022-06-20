using Geoprofs.Models;
using System;
using System.Linq;

namespace Geoprofs.Models.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DB context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.absenceTypes.Any()) // controlleren of data al aangemaakt is
            {
                return;   // DB has been seeded
            }
                // data aanmaken
            var absenceType = new AbsenceType[]
            {
            new AbsenceType{absenceName="Sick"},
            new AbsenceType{absenceName="Hospital"},
            new AbsenceType{absenceName="Dentist"},
            new AbsenceType{absenceName="Marriage"}
            };
            foreach (AbsenceType s in absenceType)
            {
                context.absenceTypes.Add(s);
            }   
            context.SaveChanges();

            var positions = new Position[]
            {
            new Position{positionName="Geo-ICT"},
            new Position{positionName="Geodesy"},
            new Position{positionName="Relation Management"},
            new Position{positionName="Finance"},
            new Position{positionName="HRM"},
            new Position{positionName="ICT"},
            new Position{positionName="CEO"},

            };
            foreach (Position c in positions)
            {
                context.positions.Add(c);
            }
            context.SaveChanges();

            var absences = new Absence[]
            {
            new Absence{totalVacation= 29},
            new Absence{totalVacation=30},
            new Absence{totalVacation=31},
            };
            foreach (Absence e in absences)
            {
                context.absences.Add(e);
            }
            context.SaveChanges();

            var Coworker = new Coworker[]
            {
            new Coworker{CoworkerName="Carson",coworkerLastname="Alexander",bsn="218952318",position=positions[2].positionId, startDate=DateTime.Parse("2005-09-01"),absence=absences[0].absenceId, vacationdays = 25},
            new Coworker{CoworkerName="Meredith",coworkerLastname="Alonso",bsn="804842313",position=positions[2].positionId,startDate=DateTime.Parse("2002-09-01"),absence=absences[1].absenceId, vacationdays = 25},
            new Coworker{CoworkerName="Arturo",coworkerLastname="Anand",bsn="885837774",position=positions[2].positionId,startDate=DateTime.Parse("2003-09-01"),absence=absences[2].absenceId, vacationdays = 25},
            new Coworker{CoworkerName="Gytis",coworkerLastname="Barzdukas",bsn="615947999",position=positions[0].positionId,startDate=DateTime.Parse("2002-09-01"),absence=absences[0].absenceId, vacationdays = 25},
            new Coworker{CoworkerName="Yan",coworkerLastname="Li",bsn="416644452",position=positions[1].positionId,startDate=DateTime.Parse("2002-09-01"),absence=absences[1].absenceId, vacationdays = 25},
            new Coworker{CoworkerName="Peggy",coworkerLastname="Justice",bsn="098765432",position=positions[3].positionId,startDate=DateTime.Parse("2001-09-01"),absence=absences[2].absenceId, vacationdays = 25},
            new Coworker{CoworkerName="Laura",coworkerLastname="Norman",bsn="865426789",position=positions[4].positionId,startDate=DateTime.Parse("2003-09-01"),absence=absences[0].absenceId, vacationdays = 25},
            new Coworker{CoworkerName="Nino",coworkerLastname="Olivetto",bsn="123412389",position=positions[5].positionId,startDate=DateTime.Parse("2005-09-01"),absence=absences[1].absenceId, vacationdays = 25},
            new Coworker{CoworkerName="Laura",coworkerLastname="Barzdukas",bsn="123412389",position=positions[6].positionId,startDate=DateTime.Parse("2005-09-01"),absence=absences[2].absenceId, vacationdays = 25}

            };

            var Supervisors = new Supervisor[]
            {
            new Supervisor{Coworker=Coworker[0].coworkerId},
            new Supervisor{Coworker=Coworker[1].coworkerId},
            new Supervisor{Coworker=Coworker[2].coworkerId},
            new Supervisor{Coworker=Coworker[8].coworkerId}
            };
            Coworker[0].supervisor = Supervisors[3].supervisorId;
            Coworker[1].supervisor = Supervisors[3].supervisorId;
            Coworker[2].supervisor = Supervisors[3].supervisorId;
            Coworker[3].supervisor = Supervisors[0].supervisorId;
            Coworker[4].supervisor = Supervisors[1].supervisorId;
            Coworker[5].supervisor = Supervisors[2].supervisorId;
            Coworker[6].supervisor = Supervisors[0].supervisorId;
            Coworker[7].supervisor = Supervisors[1].supervisorId;
            Coworker[8].supervisor = Supervisors[3].supervisorId;


            foreach (Coworker e in Coworker)
            {
                context.coworkers.Add(e);
            }
            context.SaveChanges();

            foreach (Supervisor e in Supervisors)
            {
                context.supervisors.Add(e);
            }
            context.SaveChanges();


  /*          var absenceRequest = new AbsenceRequest[]
            {
                new AbsenceRequest{Coworker = Coworker[1].coworkerId,AbsenceStart=DateTime.Parse("2022-06-10"),AbsenceEnd=DateTime.Parse("2022-06-12"),Note="I'd like to be free",absenceType=absenceType[0].absenceTypeId,absenceStatus="Onbekend"}
            };*/
/*            foreach (AbsenceRequest e in absenceRequest)
            {
                context.absenceRequests.Add(e);
            }*/
            context.SaveChanges();


            var login = new Login[]
            {
                new Login{Username="username1",Password="password",Coworker=Coworker[0].coworkerId},
                new Login{Username="username2",Password="password1",Coworker=Coworker[1].coworkerId},
                new Login{Username="username3",Password="password2",Coworker=Coworker[2].coworkerId},
                new Login{Username="username4",Password="password3",Coworker=Coworker[3].coworkerId},
                new Login{Username="username5",Password="password4",Coworker=Coworker[4].coworkerId},
                new Login{Username="username6",Password="password5",Coworker=Coworker[5].coworkerId},
                new Login{Username="username7",Password="password6",Coworker=Coworker[6].coworkerId},
                new Login{Username="username8",Password="password7",Coworker=Coworker[7].coworkerId},
                new Login{Username="username9",Password="password8",Coworker=Coworker[8].coworkerId},

            };
            foreach (Login e in login)
            {
                context.logins.Add(e);
            }
            context.SaveChanges();

        }
    }
}