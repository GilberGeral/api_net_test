namespace Coink.Models {
  public class ApiResponse{
    public required int Code { get; set; }
    public required string Message { get; set; }
    public required List<object> Data { get; set; }
  }
}
