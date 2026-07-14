using FixFlow.Application.Common;
using FixFlow.Application.DTOs;


namespace FixFlow.Application.Services;

public interface IMaintenanceRequestService
{
    Task<Result<Guid>> CreateAsync(
        CreateMaintenanceRequestDto dto,
        string customerId,
        CancellationToken cancellationToken = default);

    Task<Result<List<MaintenanceRequestDto>>> GetByCustomerIdAsync(
        string customerId,
        CancellationToken cancellationToken = default);

    Task<Result> AssignTechnicianAsync(
        Guid requestId,
        string technicianId,
        CancellationToken cancellationToken = default);

    Task<Result> AcceptAsync(
        Guid requestId,
        string technicianId,
        CancellationToken cancellationToken = default);

    Task<Result> StartWorkAsync(
        Guid requestId,
        string technicianId,
        CancellationToken cancellationToken = default);

    Task<Result> CompleteAsync(
        Guid requestId,
        string technicianId,
        CancellationToken cancellationToken = default);

    Task<Result> ConfirmByCustomerAsync(
        Guid requestId,
        string customerId,
        CancellationToken cancellationToken = default);

    //يعني الفني يشوف الطلبات المسندة له.
    Task<Result<List<MaintenanceRequestDto>>> GetByTechnicianIdAsync(
    string technicianId,
    CancellationToken cancellationToken = default);
    //يعني عرض تفاصيل طلب واحد.
    Task<Result<MaintenanceRequestDto>> GetByIdAsync(
    Guid requestId,
    string? customerId,
    string? technicianId,
    CancellationToken cancellationToken = default);

    Task<Result<PagedResult<MaintenanceRequestDto>>> GetPagedAsync(
    MaintenanceRequestQueryDto query,
    string? customerId = null,
    string? technicianId = null);
}