using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Test.Data.DataContext;
using Test.Entities;
using Dapper;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Linq;
using Test.Interfaces.Services;
using System.IO;

namespace Test.DataAccess
{
    public class ActivityRepostirory : Interfaces.Repositories.IActivityRepository
    {
		
		string Path = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"\PedidosDB.dat");

		private readonly IConnection _connection;
		

		public ActivityRepostirory(IConnection connection)
        {
            _connection = connection;
        
        }

        private DataAccess _DataAccess;
        private List<Activity> OrderList;
        private string OrderData;



        public async Task<Activity> Reschedule(Activity entity)
        {


            Read();
            int id = 1;

            if (Exist(entity.Activity_Id))
            {
                //Si existe solo actualizo
                var p = this.OrderList.First(x => x.Activity_Id == entity.Activity_Id);
                p.Activity_Status = entity.Activity_Status;

            }
            Save();
            return new Activity { Activity_Id = entity.Activity_Id, Activity_Status = entity.Activity_Status };

        }

        public async Task<Activity> GetById(int id)
        {
            Read();

            Activity entity = new Activity();
            if (Exist(id))
            {
                //Si existe 
                entity = this.OrderList.FirstOrDefault(x => x.Activity_Id == id);

            }

            Save();
            return new Activity { Activity_Id = entity.Activity_Id, Activity_Status = entity.Activity_Status, Activity_Title = entity.Activity_Title, Activity_Product = entity.Activity_Product };

        }
        public async Task<Activity> Add(Activity entity)
        {
            
            Read();
            int id = 1;

            if (Exist(entity.Activity_Id))
            {
                //Si existe solo actualizo
                var p = this.OrderList.First(x => x.Activity_Id == entity.Activity_Id);
                p.Activity_Title = entity.Activity_Title;

            }
            else
            {
                //Obtengo el id nuevo
                if (OrderList.Count > 0)
                {
                    id = this.OrderList.Max(x => x.Activity_Id) + 1;
                }
                entity.Activity_Id = id;
                //Si no existte inserta uno nuevo
                this.OrderList.Add(entity);
            }

            //Se agrega registro a la lista de ordenes
            Save();
            return new Activity { Activity_Id = id };

        }
        private void Save()
        {
            //Convierto los datos a string 
            this.OrderData = JsonConvert.SerializeObject(this.OrderList);
            //guardo los datos en el archivo
            File.WriteAllText(path: Path, this.OrderData);
           
        }
        private bool Exist(int id)
        {
            Activity persona = new Activity();
            if (OrderList.Count > 0)
            {
                persona = this.OrderList.FirstOrDefault(x => x.Activity_Id == id);
            }
            return persona?.Activity_Id > 0;
        }
        private void Read()
        {
             var _DataAccess = new DataAccess("PedidosDB.dat");
            string contenido = "";
            this.OrderData = "";
            using (StreamReader Osr = File.OpenText(path: Path))
            {
                string s = "";
                while ((s= Osr.ReadLine())!=null) {
                    this.OrderData += s;

                }
            }
                //Convierto el archivo a una lista, si es que tiene datos
                this.OrderList = this.OrderData?.Length > 0 ? JsonConvert.DeserializeObject<List<Activity>>(this.OrderData) : new List<Activity>();
        }
		

		public async Task<IEnumerable<Activity>> GetAll()
		{
            Read();

            return this.OrderList;
        }

    }
}
