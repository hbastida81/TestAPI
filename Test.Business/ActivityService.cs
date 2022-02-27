using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Business;
using Test.Entities;

namespace Test.BusinessLogic
{


	public class ActivityService : BaseService<Entities.Activity>, Interfaces.Services.IActivityServices
	{
		//Declaración de variables globales
		private readonly Interfaces.Repositories.IActivityRepository activityRepository;
		


		/**
         * <summary>Método constructor</summary>
         * <param name="taskRepository">Corresponde al tipo de interfaz de tipo ITaskRepository</param>
         */
		public ActivityService
			(
			Interfaces.Repositories.IActivityRepository activityRepository
			
			) : base(activityRepository)
		{
			this.activityRepository = activityRepository;
			
		}//Fin del método


		public async override Task<Activity> Add(Activity entity)
		{

			await base.Add(entity);

			return entity;
		}

		public async Task<Activity> Reschedule(Entities.Activity entity)
		{
			//verificamos que la activida exista
			var activity = await base.GetById(entity.Activity_Id);
			if (activity.Activity_Id == 0 || activity == null)
			{
				throw new Exception("The orders does not exist");
			}
			else if (activity.Activity_Status.Equals("Canceled"))
			{
				throw new Exception("It is not possible to processing the orders because it has been canceled");
			}

			await this.activityRepository.Reschedule(entity);
			activity = await base.GetById(entity.Activity_Id);

			return activity;

		}
	}

}
