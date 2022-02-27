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
        private List<Activity> PersonasList;
        private string PersonasData;



        public async Task<Activity> Reschedule(Activity entity)
        {


            Read();
            int id = 1;

            if (Exist(entity.Activity_Id))
            {
                //Si existe solo actualizo
                var p = this.PersonasList.First(x => x.Activity_Id == entity.Activity_Id);
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
                entity = this.PersonasList.FirstOrDefault(x => x.Activity_Id == id);

            }

            Save();
            return new Activity { Activity_Id = entity.Activity_Id, Activity_Status = entity.Activity_Status };

        }
        public async Task<Activity> Add(Activity entity)
        {
            
            Read();
            int id = 1;

            if (Exist(entity.Activity_Id))
            {
                //Si existe solo actualizo
                var p = this.PersonasList.First(x => x.Activity_Id == entity.Activity_Id);
                p.Activity_Title = entity.Activity_Title;

            }
            else
            {
                //Obtengo el id nuevo
                if (PersonasList.Count > 0)
                {
                    id = this.PersonasList.Max(x => x.Activity_Id) + 1;
                }
                entity.Activity_Id = id;
                //Si no existte inserta uno nuevo
                this.PersonasList.Add(entity);
            }

            //Agergo la persona nueva a la lista de personas
            Save();
            return new Activity { Activity_Id = id };

        }
        private void Save()
        {
            //Convierto los datos a string 
            this.PersonasData = JsonConvert.SerializeObject(this.PersonasList);
            //guardo los datos en el archivo
            File.WriteAllText(path: Path, this.PersonasData);
           
        }
        private bool Exist(int id)
        {
            Activity persona = new Activity();
            if (PersonasList.Count > 0)
            {
                persona = this.PersonasList.FirstOrDefault(x => x.Activity_Id == id);
            }
            return persona?.Activity_Id > 0;
        }
        private void Read()
        {
             var _DataAccess = new DataAccess("PedidosDB.dat");
            string contenido = "";
            this.PersonasData = "";
            using (StreamReader Osr = File.OpenText(path: Path))
            {
                string s = "";
                while ((s= Osr.ReadLine())!=null) {
                    this.PersonasData += s;

                }
            }
                //Convierto el archivo a una lista de personas, si es que tiene datos
                this.PersonasList = this.PersonasData?.Length > 0 ? JsonConvert.DeserializeObject<List<Activity>>(this.PersonasData) : new List<Activity>();
        }
		

		public async Task<IEnumerable<Activity>> GetAll()
		{
            Read();

            return this.PersonasList;
        }

	}
}
