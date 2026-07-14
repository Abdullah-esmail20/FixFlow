using FixFlow.Domain.Entities;
using FixFlow.Domain.Enums;

namespace FixFlow.Domain.Tests;

public class MaintenanceRequestWorkflowTests
{
    private static MaintenanceRequest CreateRequest()
    {
        return new MaintenanceRequest(
            customerId: "customer-1",
            title: "Air conditioner problem",
            description: "The air conditioner is not cooling properly.",
            serviceCategoryId: Guid.NewGuid(),
            location: "Apartment 12",
            preferredDate: DateTime.UtcNow.AddDays(1)
        );
    }

    [Fact]
    public void New_request_should_have_created_status()
    {
        var request = CreateRequest();

        Assert.Equal(RequestStatus.Created, request.Status);
    }

    [Fact]
    public void Created_request_can_be_assigned_to_technician()
    {
        var request = CreateRequest();

        request.AssignTechnician("technician-1");

        Assert.Equal(RequestStatus.Assigned, request.Status);
        Assert.Equal("technician-1", request.TechnicianId);
    }

    [Fact]
    public void Assigned_request_can_be_accepted()
    {
        var request = CreateRequest();
        request.AssignTechnician("technician-1");

        request.Accept();

        Assert.Equal(RequestStatus.Accepted, request.Status);
    }

    [Fact]
    public void Accepted_request_can_start_work()
    {
        var request = CreateRequest();
        request.AssignTechnician("technician-1");
        request.Accept();

        request.StartWork();

        Assert.Equal(RequestStatus.InProgress, request.Status);
    }

    [Fact]
    public void In_progress_request_can_be_completed()
    {
        var request = CreateRequest();
        request.AssignTechnician("technician-1");
        request.Accept();
        request.StartWork();

        request.Complete();

        Assert.Equal(RequestStatus.Completed, request.Status);
    }

    [Fact]
    public void Completed_request_can_be_confirmed_by_customer()
    {
        var request = CreateRequest();
        request.AssignTechnician("technician-1");
        request.Accept();
        request.StartWork();
        request.Complete();

        request.ConfirmByCustomer();

        Assert.Equal(RequestStatus.CustomerConfirmed, request.Status);
    }

    [Fact]
    public void Cannot_accept_request_before_assignment()
    {
        var request = CreateRequest();

        Assert.Throws<InvalidOperationException>(() => request.Accept());
    }

    [Fact]
    public void Cannot_complete_request_before_start_work()
    {
        var request = CreateRequest();
        request.AssignTechnician("technician-1");
        request.Accept();

        Assert.Throws<InvalidOperationException>(() => request.Complete());
    }
}
// These tests verify the maintenance request workflow rules.
// They make sure that a request moves through the correct statuses
// and prevents invalid actions, such as completing a request before work starts.

// تختبر هذه الحالات قواعد دورة حياة طلب الصيانة.
// وتتحقق من انتقال الطلب بين الحالات المسموح بها فقط،
// مع التأكد من منع العمليات غير المنطقية مثل
// إكمال الطلب قبل بدء تنفيذ أعمال الصيانة.git add README.md
