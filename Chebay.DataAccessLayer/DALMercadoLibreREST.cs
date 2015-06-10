using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Shared.Entities;

namespace DataAccessLayer
{
    public class DALMercadoLibreREST
    {
        private HttpClient _client = new HttpClient();
        public DALMercadoLibreREST()
        {
            _client.BaseAddress = new Uri("https://api.mercadolibre.com/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        //pasar dynob.categories
        public Dictionary<string,string> ListarCategoriasSitio(string sitio)
        {

            dynamic json = ripJson("/sites/"+sitio);
            Dictionary<string,string> ret = new Dictionary<string,string>();
            foreach (var i in json.categories)
            {
                ret.Add((string)i.id, (string)i.name);
            }
            return ret;
        }


        public dynamic ripJson(string url)
        {
            Task<HttpResponseMessage> taskresponse = _client.GetAsync(url);
            taskresponse.Wait();
            HttpResponseMessage response = taskresponse.Result;
            if (response.IsSuccessStatusCode)
            {
                Task<string> task = response.Content.ReadAsStringAsync();
                task.Wait();
                string content = task.Result;
                var dynob = JsonConvert.DeserializeObject<dynamic>(content);
                return dynob;
            }
            return null;
        }


        public Dictionary<string, int> listarCategoriasHijas(string categoria)
        {
            return null;
        }


        //parametro json /categories/id_cat
        public Dictionary<string, Dictionary<string, string>> getCategoriesSons(string categoria)
        {
            var mycategories = new Dictionary<string, Dictionary<string, string>>();
            List<string> stack = new List<string>();
            stack.Add(categoria);
            while (stack.Count != 0)
            {
                string first = stack.First();
                dynamic json = ripJson("/categories/"+first);
                stack.RemoveAt(0);
                System.Console.WriteLine(json.id);
                if (!mycategories.ContainsKey(first))
                {
                    mycategories.Add(first, new Dictionary<string, string>());
                
                    foreach (var cat in json.children_categories)
                    {
                        if (!mycategories.ContainsKey((string)cat.id))
                        {
                            mycategories[first].Add((string)cat.id, (string)cat.name);
                            stack.Add((string)cat.id);
                        }             
                        //System.Console.WriteLine((string)cat.id);
                    }
                }
            }
            return mycategories;
        }


        public void getProductsByCategory(string category)
        {
            List<Producto> productos = new List<Producto>();
            dynamic json = ripJson("/sites/MLU/search?category="+category);
            foreach (var p in json.results)
            {
                string categoria = (string)p.category_id;
                string nombre = (string)p.name;
                int price = (int)double.Parse((string)p.price);
                string latitud = (string)p.seller_address.latitude;
                string longitud = (string)p.seller_address.longitude;
                DateTime fecha_cierre = Convert.ToDateTime((string)p.stop_time);
                string id_vendedor = (string)p.seller.id;
                //faltan imagenes
                //buscar categoria
                //crear usuario
                Producto producto = new Producto { 
                                                    fecha_cierre=fecha_cierre,
                                                    latitud = latitud,
                                                    longitud = longitud,
                                                    nombre = nombre,
                                                    precio_compra = price,
                                                    UsuarioID="mercadolibre-"+id_vendedor,
                                                    //imagenes
                                                    //categoria
                                                    };
                productos.Add(producto);
                System.Console.WriteLine((string)p.title + fecha_cierre.ToString());
            }
        }

        public void test()
        {
            //var cat = ListarCategoriasSitio("MLA");
            //foreach (var i in cat)
            //{
            //    System.Console.WriteLine(i);
            //}
            //var dic = getCategoriesSons("MLA7076");//("MLA126844");//("MLA1648");
            getProductsByCategory("MLA7076");
        
            //pasos:
            //armo categorias
            //armo productos lista
            //de lo anterior voy agregando los usuarios mercadolibre-userid
            //obtenerImagenesProducto(idproducto)
            //createUser(..)
        }
       
    }
}
