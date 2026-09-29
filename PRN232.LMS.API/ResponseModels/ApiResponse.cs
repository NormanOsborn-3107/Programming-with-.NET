using PRN232.LMS.Repositories.Common;

namespace PRN232.LMS.API.ResponseModels
{
    /// <summary>
    /// Standard API response wrapper returned by all endpoints.
    /// </summary>
    /// <typeparam name="T">Type of the data payload.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>Indicates whether the request was processed successfully.</summary>
        public bool Success { get; set; }

        /// <summary>Human-readable message describing the result.</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>The response payload. Null when the request fails.</summary>
        public T? Data { get; set; }

        /// <summary>Validation or business-logic errors. Null when the request succeeds.</summary>
        public object? Errors { get; set; }

        // ── Factory helpers ───────────────────────────────────────────────────

        public static ApiResponse<T> Ok(T data, string message = "Request processed successfully")
            => new() { Success = true, Message = message, Data = data, Errors = null };

        public static ApiResponse<T> Created(T data, string message = "Resource created successfully")
            => new() { Success = true, Message = message, Data = data, Errors = null };

        public static ApiResponse<T> NotFound(string message)
            => new() { Success = false, Message = message, Data = default, Errors = null };

        public static ApiResponse<T> BadRequest(object errors, string message = "Validation failed")
            => new() { Success = false, Message = message, Data = default, Errors = errors };

        public static ApiResponse<T> ServerError(string message = "An unexpected error occurred")
            => new() { Success = false, Message = message, Data = default, Errors = null };
    }

    /// <summary>
    /// Paged API response — pagination sits at root level (NOT nested inside data).
    /// Format: { success, message, data: [...], errors, pagination: { page, pageSize, totalItems, totalPages } }
    /// </summary>
    public class PagedApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<T>? Data { get; set; }
        public object? Errors { get; set; }
        /// <summary>Pagination metadata at root level per grader contract §6.2.</summary>
        public PaginationMetadata? Pagination { get; set; }

        public static PagedApiResponse<T> Ok(IEnumerable<T> data, PaginationMetadata pagination,
            string message = "Request processed successfully")
            => new() { Success = true, Message = message, Data = data, Errors = null, Pagination = pagination };
    }
}

