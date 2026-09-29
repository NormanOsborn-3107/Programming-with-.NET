using Microsoft.Extensions.DependencyInjection;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Implementations;

namespace PRN232.LMS.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<ISemesterService, SemesterService>();
            services.AddScoped<ISubjectService, SubjectService>();
            return services;
        }
    }
}
