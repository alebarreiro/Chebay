﻿using MongoDB.Bson;
using MongoDB.Driver;
using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class MongoDB
    {
        private MongoClient _client;
        private string _database = "MongoLab-b";
        public MongoDB()
        {
            string connection = "mongodb://MongoLab-b:ehWPn5SeNey2phicvNCu.dGxHaAlBo.8ag7Uq16.3sU-@ds036698.mongolab.com:36698/MongoLab-b";
            _client = new MongoClient(connection);
        }

        public async Task createIndexRecomendation(string TiendaID)
        {
            var db = _client.GetDatabase(_database);
            var collection = db.GetCollection<DataRecomendacion>(TiendaID);
            await collection.Indexes.CreateOneAsync(Builders<DataRecomendacion>.IndexKeys.Ascending(_ => _.UsuarioID));
        }


        public async Task InsertProducts(string TiendaID, DataRecomendacion dataRecomendacion)
        {
            var db = _client.GetDatabase(_database);
            var collection = db.GetCollection<DataRecomendacion>(TiendaID);
            var docs = collection.Find(new BsonDocument()).FirstOrDefaultAsync();

            await collection.ReplaceOneAsync(d => d.UsuarioID == dataRecomendacion.UsuarioID, 
                dataRecomendacion, new UpdateOptions { IsUpsert = true });           
        }
        public DataRecomendacion GetRecomendacionesUsuario(string TiendaID, DataRecomendacion dataRecomendacion)
        {
            var db = _client.GetDatabase(_database);
            var collection = db.GetCollection<BsonDocument>(TiendaID);
            var query =  collection.Find(new BsonDocument("UsuarioID", dataRecomendacion.UsuarioID));
            var count = query.CountAsync();
            count.Wait();
            if(count.Result == 0)
                return null;

            var prod= query.FirstAsync();
            
            List<DataProducto> listDP = new List<DataProducto>();
            string UsuarioID = prod.Result["UsuarioID"].AsString;
            BsonArray ar = prod.Result["productos"].AsBsonArray;
            
            string nombre = "";
            long productoid = 0;
            string descripcion = "";
            int precio_base_subasta = 0;
            int precio_compra = 0;
            DateTime fecha_cierre = DateTime.UtcNow;
            string idOfertante = "";
            foreach (var a in ar)
            {
                if(!a["nombre"].IsBsonNull)
                    nombre = a["nombre"].AsString;
                if(!a["ProductoID"].IsBsonNull)
                    productoid = a["ProductoID"].AsInt64;
                if(!a["descripcion"].IsBsonNull)
                    descripcion = a["descripcion"].AsString;
                if (!a["precio_base_subasta"].IsBsonNull)
                    precio_base_subasta = a["precio_base_subasta"].AsInt32;
                if (!a["precio_compra"].IsBsonNull)
                    precio_compra = a["precio_compra"].AsInt32;
                if (!a["fecha_cierre"].IsBsonNull)
                    fecha_cierre = a["fecha_cierre"].ToUniversalTime();
                if (!a["idOfertante"].IsBsonNull)
                    idOfertante = a["idOfertante"].AsString;

                DataProducto dp = new DataProducto {descripcion=descripcion,
                                                    fecha_cierre=fecha_cierre,
                                                    idOfertante=idOfertante,
                                                    nombre=nombre,
                                                    precio_compra=precio_compra,
                                                    precio_base_subasta=precio_base_subasta,
                                                    ProductoID=productoid 
                                                    };
                listDP.Add(dp);
            }
            return new DataRecomendacion { UsuarioID=UsuarioID, productos=listDP };
        }

    }
}
