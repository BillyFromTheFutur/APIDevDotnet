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
using Moq;

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

        [TestMethod]
        public void GetUtilisateurById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            var actionResult = userController.GetUtilisateurById(1).Result;
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(user, actionResult.Value as Utilisateur);
        }

        [TestMethod]
        public void GetUtilisateurById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            var actionResult = userController.GetUtilisateurById(0).Result;
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public async Task GetByEmailSuccessTest()
        {
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByStringAsync("clilleymd@last.fm").Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            var actionResult = userController.GetUtilisateurByEmail("clilleymd@last.fm").Result;
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(user, actionResult.Value as Utilisateur);
        }

        [TestMethod()]
        public async Task GetByEmailFailTest()
        {

            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            var actionResult = userController.GetUtilisateurByEmail("").Result;
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));

        }


        [TestMethod]
        public void Postutilisateur_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            Utilisateur user = new Utilisateur
            {
                Nom = "POISSON",
                Prenom = "Pascal",
                Mobile = "1",
                Mail = "poisson@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act
            var actionResult = userController.PostUtilisateur(user).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Utilisateur>), "Pas un ActionResult<Utilisateur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Utilisateur), "Pas un Utilisateur");
            user.UtilisateurId = ((Utilisateur)result.Value).UtilisateurId;
            Assert.AreEqual(user, (Utilisateur)result.Value, "Utilisateurs pas identiques");
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

        [TestMethod]
        public void DeleteUtilisateurTest_AvecMoq()
        {
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            var actionResult = userController.DeleteUtilisateur(1).Result;
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); 
        }


        [TestMethod]
        public async Task PutUtilisateurTest_AvecMoq()
        {
            
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 21,
                Nom = "Test",
                Prenom = "Test",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            Utilisateur userModified = new Utilisateur
            {
                UtilisateurId = 21,
                Nom = "TestModifier",
                Prenom = "TestModifier",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(21).Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            //mockRepository.Setup(x => x.UpdateAsync(user, userModified).Result).Returns(userModified);
            if(userController.GetUtilisateurById(21).Result != null)
            {
                await userController.DeleteUtilisateur(21);

            }
            var actionResult = userController.PostUtilisateur(userModified).Result;

            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Utilisateur>), "Pas un ActionResult<Utilisateur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Utilisateur), "Pas un Utilisateur");
            user.UtilisateurId = ((Utilisateur)result.Value).UtilisateurId;
            Assert.AreNotEqual(user, (Utilisateur)result.Value, "Utilisateurs pas identiques");
        }
    }
}
