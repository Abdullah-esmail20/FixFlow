import { useEffect, useState } from "react";
import { getCurrentUser, logoutUser } from "../services/authService";
import { getAdminMaintenanceRequests } from "../services/maintenanceRequestService";
import type { MaintenanceRequest } from "../types/maintenanceRequest";
import "./DashboardPage.css";

function AdminDashboardPage() {
  const user = getCurrentUser();

  const [requests, setRequests] = useState<MaintenanceRequest[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  function handleLogout() {
    logoutUser();
    window.location.href = "/";
  }

  useEffect(() => {
    async function loadRequests() {
      try {
        const result = await getAdminMaintenanceRequests();
        setRequests(result.items);
      } catch {
        setError("Failed to load maintenance requests.");
      } finally {
        setLoading(false);
      }
    }

    loadRequests();
  }, []);

  return (
    <div className="dashboard-page">
      <div className="dashboard-header">
        <div>
          <h1>Admin Dashboard</h1>
          <p>Welcome, {user?.fullName}</p>
        </div>

        <button onClick={handleLogout}>Logout</button>
      </div>

      <div className="dashboard-card">
        <h2>Maintenance Requests</h2>
        <p>View, search, filter, sort, and assign maintenance requests.</p>

        {loading && <p>Loading requests...</p>}

        {error && <div className="error-message">{error}</div>}

        {!loading && !error && (
          <table className="requests-table">
            <thead>
              <tr>
                <th>Title</th>
                <th>Status</th>
                <th>Priority</th>
                <th>Location</th>
                <th>Created At</th>
              </tr>
            </thead>

            <tbody>
              {requests.map((request) => (
                <tr key={request.id}>
                  <td>{request.title}</td>
                  <td>{request.status}</td>
                  <td>{request.priority}</td>
                  <td>{request.location ?? "-"}</td>
                  <td>{new Date(request.createdAt).toLocaleDateString()}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}

        {!loading && !error && requests.length === 0 && (
          <p>No maintenance requests found.</p>
        )}
      </div>
    </div>
  );
}

export default AdminDashboardPage;