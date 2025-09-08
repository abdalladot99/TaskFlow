using AutoMapper;
using TaskFlow.Application.DTOs.ApplicationUserDTO;
using TaskFlow.Application.DTOs.Category;
using TaskFlow.Application.DTOs.CategoryDTO;
using TaskFlow.Application.DTOs.PriorityDto;
using TaskFlow.Application.DTOs.RecurrenceTypeDTO;
using TaskFlow.Application.DTOs.StatusDTO;
using TaskFlow.Application.DTOs.TaskCollaboratiorDTO;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Application.Queries.NotificationsQueries;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Application.Mapping
{
	internal class MappingProfile:Profile//والوراثه هنا من كلاس موجود في المكتبه  
	{
		public MappingProfile()//is constructor
		{

 			CreateMap<AddTaskDTO, AppTask>();
			CreateMap<AppTask, AddTaskDTO>();

			CreateMap<AddCategoryDTO, Category>();
			CreateMap<Category, AddCategoryDTO>();

			CreateMap<EditCategoryDTO, Category>();
			CreateMap<Category, EditCategoryDTO>();

			CreateMap<ApplicationUser, RegisterUserDTO>();
			CreateMap<RegisterUserDTO, ApplicationUser>();

			CreateMap<AddPriorityDto, Priority>();
			CreateMap<Priority, AddPriorityDto>();

			CreateMap<Status, AddStatusDto>();
			CreateMap<AddStatusDto, Status>();
			 
 			CreateMap<AddRecurrenceTypeDto, RecurrenceType>();
 			CreateMap<RecurrenceType, AddRecurrenceTypeDto>();

 			CreateMap<DataTaskDTO, AppTask>();
 			CreateMap<AppTask, DataTaskDTO>();

 			CreateMap<Notification, NotificationDTO>();
 			CreateMap<NotificationDTO, Notification>();
 
			CreateMap<Priority, DataPriorityDTO>().ReverseMap();

			CreateMap<Status, DataStatusDTO>().ReverseMap();

			CreateMap<Category, DataCategoryDTO>().ReverseMap();

			CreateMap<BigDataViewTaskDTO, AppTask>().ReverseMap();

			CreateMap<BigDataViewTaskDTO, TaskCollaborator>().ReverseMap();

			CreateMap<ViewTaskCollaboratorsMeDTO, TaskCollaborator>().ReverseMap();

			CreateMap<TaskCollaborator, CollaboratorDTO>().ReverseMap();

			CreateMap<NotificationDTO, Notification>().ReverseMap();

			CreateMap<DataRecurrenceTypeDTO,RecurrenceType>().ReverseMap();
			 





		} 
	}
}
