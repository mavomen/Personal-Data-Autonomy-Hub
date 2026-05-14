using MediatR;

namespace PDH.ApplicationTests;

public class TestRequest : IRequest<bool>
{
    public string Name { get; set; } = string.Empty;
}
