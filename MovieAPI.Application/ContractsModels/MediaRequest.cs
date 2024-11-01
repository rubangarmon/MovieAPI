namespace MovieAPI.Application.ContractsModels
{
    public record MediaRequest
    {
        public required string Name { get; set; }
        public int Page { get; set; } = 1;
    }
}
