using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Data.Entity;
using Shared.Entities;
using Shared.DataTypes;
using MongoDB.Bson.Serialization;

namespace DataAccessLayer
{
    public class MongoDB
    {
        private MongoClient _client;
        public MongoDB()
        {
            string connection = "mongodb://chebaylinux.cloudapp.net:27017";
            _client = new MongoClient(connection);
        }

        public async Task createIndexRecomendation(string TiendaID)
        {          
            var db = _client.GetDatabase("recomendaciones");
            var collection = db.GetCollection<DataRecomendacion>(TiendaID);
            await collection.Indexes.CreateOneAsync(Builders<DataRecomendacion>.IndexKeys.Ascending(_ => _.UsuarioID));
        }


        public async Task InsertProducts(string TiendaID, DataRecomendacion dataRecomendacion)
        {
            var db = _client.GetDatabase("recomendaciones");
            var collection = db.GetCollection<DataRecomendacion>(TiendaID);
            var docs = collection.Find(new BsonDocument()).FirstOrDefaultAsync();

            await collection.ReplaceOneAsync(d => d.UsuarioID == dataRecomendacion.UsuarioID, 
                dataRecomendacion, new UpdateOptions { IsUpsert = true });           
        }
        public DataRecomendacion GetRecomendacionesUsuario(string TiendaID, DataRecomendacion dataRecomendacion)
        {
            var db = _client.GetDatabase("recomendaciones");
            var collection = db.GetCollection<BsonDocument>(TiendaID);
            var prod =  collection.Find(new BsonDocument("UsuarioID", dataRecomendacion.UsuarioID))//(d => d.UsuarioID==dataRecomendacion.UsuarioID)
                       .FirstAsync();

            List<DataProducto> listDP = new List<DataProducto>();
            string UsuarioID = prod.Result["UsuarioID"].AsString;
            BsonArray ar = prod.Result["productos"].AsBsonArray;
            
            foreach (var a in ar)
            {
                string nombre = a["nombre"].AsString;
                long productoid = a["ProductoID"].AsInt64;
                string descripcion = a["descripcion"].AsString;
                int precio_base_subasta = a["precio_base_subasta"].AsInt32;
                int precio_compra = a["precio_compra"].AsInt32;
                DateTime fecha_cierre = a["fecha_cierre"].ToUniversalTime();
                string idOfertante = a["idOfertante"].AsString;
                DataProducto dp = new DataProducto {descripcion=descripcion, fecha_cierre=fecha_cierre, idOfertante=idOfertante,
                                                     nombre=nombre, precio_compra=precio_compra,  precio_base_subasta=precio_base_subasta,
                                                     ProductoID=productoid };
                listDP.Add(dp);
            }
            return new DataRecomendacion {  UsuarioID=UsuarioID, productos=listDP };// prod.Result;
        }

    }
}
