using Microsoft.VisualStudio.TestTools.UnitTesting;
using API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
using System.Xml;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using API.Models.DataManager;
using API.Models.Repository;

namespace API.Controllers.Tests
{
    [TestClass()]
    public class UtilisateursControllerTests
    {
        private UtilisateursController controller;
        private SeriesDbContext _context;
        private IDataRepository<Utilisateur> dataRepository;
        public UtilisateursControllerTests()
        {
            var builder = new DbContextOptionsBuilder<SeriesDbContext>().UseNpgsql("Server=localhost;port=5432;Database=SeriesDB;uid=postgres;password=postgres;");
            _context = new SeriesDbContext(builder.Options);
            dataRepository = new UtilisateurManager(_context);
            controller = new UtilisateursController(dataRepository);
        }


        [TestMethod()]
        public async Task GetUtilisateursTest()
        {

            var result = await controller.GetUtilisateurs();
            var userList = _context.Utilisateurs.ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(userList.Count, result.Value.Count());
        }

        [TestMethod()]
        public async Task GetByIDSuccessTest()
        {
            int idUser = 22;
            var result = await controller.GetUtilisateurById(22);
            var userInDB = _context.Utilisateurs.Where(c => c.UtilisateurId == idUser).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(userInDB, result.Value);
        }

        [TestMethod()]
        public async Task GetByIDFailTest()
        {

            int idUser = 2220;
            var result = await controller.GetUtilisateurById(idUser);
            
            Assert.IsNull(result.Value);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            Assert.IsInstanceOfType(result.Result, typeof(Utilisateur));

        }

        [TestMethod()]
        public async Task GetByEmailSuccessTest()
        {
            string userEmail = "abramford2@businesswire.com";
            var result = await controller.GetUtilisateurByEmail(userEmail);
            var userInDB = _context.Utilisateurs.Where(c => c.Mail == userEmail).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(userInDB, result.Value);
        }

        [TestMethod()]
        public async Task GetByEmailFailTest()
        {

            string userEmail = "fake@email.com";
            var result = await controller.GetUtilisateurByEmail(userEmail);

            Assert.IsNull(result.Value);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            Assert.IsInstanceOfType(result.Result, typeof(Utilisateur));

        }


        [TestMethod]
        public void Postutilisateur_ModelValidated_CreationOK()
        {
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
             Utilisateur userAtester = new Utilisateur()
             {
                 Nom = "MACHIN",
                 Prenom = "Luc",
                 Mobile = "0606070809",
                 Mail = "machin" + chiffre + "@gmail.com",
                 Pwd = "Toto1234!",
                 Rue = "Chemin de Bellevue",
                 CodePostal = "74940",
                 Ville = "Annecy-le-Vieux",
                 Pays = "France",
                 Latitude = null,
                 Longitude = null
             };
            var result = controller.PostUtilisateur(userAtester).Result; 
            Utilisateur? userRecupere = _context.Utilisateurs.Where(u => u.Mail.ToUpper() == userAtester.Mail.ToUpper()).FirstOrDefault();
            userAtester.UtilisateurId = userRecupere.UtilisateurId;
            Assert.AreEqual(userRecupere, userAtester, "Utilisateurs pas identiques");
        }

        [TestMethod]
        public void Postutilisateur_Email_Dupliquer()
        {
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            Utilisateur user = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            var result = controller.PostUtilisateur(user).Result;
            Utilisateur userDupliquer = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            //result = controller.PostUtilisateur(userDupliquer).Result;
            Assert.ThrowsException<System.AggregateException>(() => controller.PostUtilisateur(userDupliquer).Result);
        }

        //[TestMethod]
        //public void Postutilisateur_Champ_Nom_Omis()
        //{
        //    Random rnd = new Random();
        //    int chiffre = rnd.Next(1, 1000000000);
        //    Utilisateur user = new Utilisateur()
        //    {
        //        Nom = "MACHIN",
        //        Prenom = "Luc",
        //        Mail = "machin" + chiffre + "@gmail.com",
        //        Pwd = "Toto1234!",
        //        Mobile = "0606070809",
        //        Rue = "Chemin de Bellevue",
        //        CodePostal = "74940",
        //        Ville = "Annecy-le-Vieux",
        //        Pays = "France",
        //        Latitude = null,
        //        Longitude = null
        //    };
        //    Assert.ThrowsException<System.AggregateException>(() => controller.PostUtilisateur(user).Result);
        //}

        [TestMethod]
        public void Postutilisateur_Regex()
        {
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            Utilisateur utilisateur = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "1",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            string PhoneRegex = @"^0[0-9]{9}$";
            Regex regex = new Regex(PhoneRegex);
            if (!regex.IsMatch(utilisateur.Mobile))
            {
                controller.ModelState.AddModelError("Mobile", "Le n° de mobile doit contenir 10 chiffres"); 
            }
            var result = controller.PostUtilisateur(utilisateur).Result;
            Assert.AreEqual(null,result.Value);
        }
    }
}
