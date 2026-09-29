using Microsoft.Extensions.DependencyInjection;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Implementations;

namespace PRN232.LMS.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<ISemesterRepository, SemesterRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            return services;
        }
    }
}
