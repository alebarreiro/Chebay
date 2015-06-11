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
using Shared.DataTypes;
using System.Diagnostics;
using System.Net;

namespace DataAccessLayer
{
    public class DALMercadoLibreREST:IDALMercadoLibreREST
    {
        private HttpClient _client = new HttpClient();
        public DALMercadoLibreREST()
        {
            _client.BaseAddress = new Uri("https://api.mercadolibre.com/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        //pasar dynob.categories
        public List<DataCategoria> ListarCategoriasSitio(string sitio)
        {         
            dynamic json = ripJson("/sites/"+sitio);
            List<DataCategoria> ret = new List<DataCategoria>();
            foreach (var i in json.categories)
            {
                ret.Add(new DataCategoria { id=(string)i.id, name = (string)i.name });
            }
            return ret;
        }


        private dynamic ripJson(string url)
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


        public List<DataCategoria> listarCategoriasHijas(string categoria)
        {
            List<DataCategoria> lista = new List<DataCategoria>();
            dynamic json = ripJson("/categories/" + categoria);
            foreach (var cat in json.children_categories)
            {
                lista.Add(new DataCategoria 
                                    {id=(string)cat.id,
                                     name=(string)cat.name,
                                     total_items_in_this_category = int.Parse((string)cat.total_items_in_this_category)
                                    });
            }
            return lista;
        }


        //parametro json /categories/id_cat
        private Dictionary<string, Dictionary<string, string>> getCategoriesSons(string categoria)
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

        private void CrearUsuarioML(string TiendaID)
        {
            string usermail = "chebaysend@gmail.com";
            Usuario u = new Usuario {   Nombre="MercadoLibreWebscraping",
                                        Pais="Uruguay",
                                        Email=usermail,
                                        UsuarioID=usermail,
                                        fecha_ingreso= DateTime.UtcNow
                                        };
            using (var db = ChebayDBContext.CreateTenant(TiendaID))
            {
                db.usuarios.Add(u);
                db.SaveChanges();
            }
        }

        private List<ImagenProducto> ObtenerImagenesProducto(long ProductoID, string item)
        {
            List<ImagenProducto> lista = new List<ImagenProducto>();
            var json = ripJson("/items/" + item);
            foreach (var picture in json.pictures)
            {

                var webCli = new WebClient();
                byte[] bytes = webCli.DownloadData((string)picture.url);
                ImagenProducto im = new ImagenProducto { ProductoID=ProductoID, Imagen = bytes};
                lista.Add(im);
            }
            return lista; 
        }

        public void ObtenerProductosMLporCategoria(string TiendaID, string limit, string categoryML, long categoryLocal)
        {
            IDALSubasta sdal = new DALSubastaEF();

            string user_ml = "chebaysend@gmail.com";
            //verifico si usuario existe
            using (var db = ChebayDBContext.CreateTenant(TiendaID))
            {
                var query = from u in db.usuarios
                            where u.UsuarioID == user_ml
                            select u;
                if (query.Count() == 0)
                {
                    CrearUsuarioML(TiendaID);
                }


                List<Producto> productos = new List<Producto>();
                dynamic json = ripJson("/sites/MLU/search?limit=" + limit + "&category=" + categoryML);

                int total = 0;
                foreach (var p in json.results)
                {
                    //string categoria = ;
                    string nombre = (string)p.title;
                    int price = (int)double.Parse((string)p.price);
                    int subasta = price / 2;
                    string latitud = (string)p.seller_address.latitude;
                    string longitud = (string)p.seller_address.longitude;
                    DateTime fecha_cierre = Convert.ToDateTime((string)p.stop_time);
                    //string id_vendedor = (string)p.seller.id;
                    Producto producto = new Producto
                    {
                        fecha_cierre = fecha_cierre,
                        latitud = latitud,
                        longitud = longitud,
                        nombre = nombre,
                        precio_compra = price,
                        precio_base_subasta =subasta,
                        UsuarioID = user_ml,
                        CategoriaID = categoryLocal,
                        imagenes = new List<ImagenProducto>()
                    };
                    long idprod = sdal.AgregarProducto(producto, TiendaID);
                    //agrego producto
                    
                    var imagenes = ObtenerImagenesProducto(idprod, (string)p.id);
                    foreach (var im in imagenes)
                    {
                        sdal.AgregarImagenProducto(im, TiendaID);
                    }

                    total++;
                    Debug.WriteLine(producto.nombre+" "
                                            +producto.UsuarioID+" "
                                            +producto.latitud + " "
                                            +producto.longitud + " "
                                            +producto.precio_compra + " "
                                            +producto.CategoriaID
                                            );
                }
                Debug.WriteLine("Total: " + total);

            }
        }

        public void test()
        {
            ObtenerProductosMLporCategoria("MobileCenter", "2", "MLU3502", 6);
            //var cat = ListarCategoriasSitio("MLA");
            //foreach (var i in cat)
            //{
            //    System.Console.WriteLine(i);
            //}
            //var dic = getCategoriesSons("MLA7076");//("MLA126844");//("MLA1648");
            //getProductsByCategory("MLA7076");
        
            //pasos:
            //armo categorias
            //armo productos lista
            //de lo anterior voy agregando los usuarios mercadolibre-userid
            //obtenerImagenesProducto(idproducto)
            //createUser(..)
        }
       
    }
}
