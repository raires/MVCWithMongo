using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCWithMongo.Models;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace MVCWithMongo.Controllers
{
    public class PrimaryController : Controller
    {

        private string MongoServerName = "Server=localhost:27017";
        private string MongoDBName = "MongoMVC";
        private string MongoDocumentPrimaryName = "Primary";
        MongoServer objServer = null;
        MongoDatabase objDatabse = null;
        MongoCollection<BsonDocument> PrimaryDetails = null;

        private void PrimaryControllerStart()
        {
            //Connect to MongoDB
            objServer = MongoServer.Create(MongoServerName);
            //database name( if exists connect  if not create it will create automatically a database
            objDatabse = objServer.GetDatabase(MongoDBName);

            //Document/table Name( if exists it select else it will create a document/table 
            PrimaryDetails = objDatabse.GetCollection<BsonDocument>(MongoDocumentPrimaryName);
        }





        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(DocumentPrimaryModel model)
        {
            PrimaryControllerStart();

            //Insert into Primary Document/table.
            BsonDocument objDocument = new BsonDocument {
                {"ID",model.ID},
                {"Name",model.Name}
            };

            PrimaryDetails.Insert(objDocument);
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            PrimaryControllerStart();

            try
            {

                List<DocumentPrimaryModel> PrimaryDetails = objDatabse.GetCollection<DocumentPrimaryModel>(MongoDocumentPrimaryName).FindAll().ToList();
                return View(PrimaryDetails);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return View();
        }

        public ActionResult Delete(object id)
        {
            PrimaryControllerStart();

            IMongoQuery query = Query.EQ("_id", ObjectId.Parse(id.ToString()));
            objDatabse.GetCollection<DocumentPrimaryModel>(MongoDocumentPrimaryName).Remove(query);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(object id)
        {
            PrimaryControllerStart();

            IMongoQuery query = Query.EQ("_id", ObjectId.Parse(id.ToString()));
            DocumentPrimaryModel primary = objDatabse.GetCollection<DocumentPrimaryModel>(MongoDocumentPrimaryName).Find(query).FirstOrDefault();
            return View(primary);
        }

        [HttpPost]
        public ActionResult Edit(DocumentPrimaryModel model)
        {
            PrimaryControllerStart();            

            JObject obj = JObject.Parse(model._id.ToJson());
            
            IMongoQuery query = Query.EQ("_id", ObjectId.Parse((string)obj["_v"].First));

            IMongoUpdate  updateQuery = Update.Set("Name", model.Name);
            objDatabse.GetCollection<DocumentPrimaryModel>(MongoDocumentPrimaryName).Update(query, updateQuery);
            return RedirectToAction("Index");
        }


    }
}
