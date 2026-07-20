import { getCurrentUser, logoutUser } from "../services/authService";
import "./DashboardPage.css";

function AdminDashboardPage() {
  const user = getCurrentUser();

  function handleLogout() {
    logoutUser();
    window.location.href = "/";
  }

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
        <p>
          Here the admin will view, search, filter, sort, and assign maintenance
          requests.
        </p>
      </div>
    </div>
  );
}

export default AdminDashboardPage;