using AutoMapper;
using Server.Domain;
using Server.Models;

namespace Server.Mapper
{
    public class ConfigureDTOS:Profile
    {
        public ConfigureDTOS()
        {
            CreateMap<Personal, PersonalModel>().ReverseMap();
            CreateMap<PelicanHRMTenant, PelicanHRMTenantModel>().ReverseMap();
            CreateMap<CandidateInfo, CandidateModel>().ReverseMap();
            CreateMap<Education, EducationModel>().ReverseMap();
            CreateMap<JobExperience, JobExperienceModel>().ReverseMap();
            CreateMap<ProfessionalLicense, ProfessionalLicenseModel>().ReverseMap();
            CreateMap<GENERALTASK, GeneralTaskModel>().ReverseMap();
            CreateMap<NOTIFICATIONS, NotificationsModel>().ReverseMap();
            CreateMap<Asset, AssetModel>().ReverseMap();
            CreateMap<Attachments, AttachmentModel>().ReverseMap();
            CreateMap<Case, CaseModel>().ReverseMap();
           
            CreateMap<CONTACTDETAILS, ContactDetailModel>().ReverseMap();
            CreateMap<EmergencyContacts, EmergencyContactModel>().ReverseMap();
            CreateMap<HRNotes, HRNoteModel>().ReverseMap();
            CreateMap<Dependent, DependentModel>().ReverseMap();
            CreateMap<ZoomMeetings, ZooMeetingModel>().ReverseMap();
            CreateMap<WebPages, WebPagesModel>().ReverseMap();
            CreateMap<ContactPage, ContactModel>().ReverseMap();
            CreateMap<BlogPage, BlogModel>().ReverseMap();
            // Map Trainings to TrainingModel excluding the Id for creation
            CreateMap<Trainings, TrainingModel>()
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id != default));
            CreateMap<CaseCommentModel, CaseComment>()
          .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                                                           
          .ReverseMap(); 

            CreateMap<TrainingModel, Trainings>();
            CreateMap<Designations, DesginationModel>().ReverseMap();
            CreateMap<Chat, ChatModel>().ReverseMap();

        }
    }
}