using AutoMapper;
using BookStore.DTOModels;
using Model.Entites;

namespace BookStore.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookResponse>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author!.Name));
            CreateMap<UpdateBookRequest, Book>();
            CreateMap<CreateBookRequest, Book>();

        }
    }
}
