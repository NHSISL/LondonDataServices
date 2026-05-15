// ---
// skill: the-standard-reacttypescript-routing
// type: example
// source-section: "11. Routing"
// demonstrates: "tsr-routing-001, tsr-routing-002, tsr-routing-003, tsr-routing-004"
// ---

import { Routes, Route, Navigate } from "react-router-dom";
import { DashboardPage } from "../pages/dashboard/DashboardPage";
import { PatientProfilePage } from "../pages/patients/PatientProfilePage";
import { AdminPage } from "../pages/admin/AdminPage";
import { ProtectedRoute } from "./ProtectedRoute";

// ✅ Centralized route file — all routes in one place (tsr-routing-002)
export function AppRoutes() {
    return (
        <Routes>
            {/* ✅ Routes point to pages — not cards or lists (tsr-routing-001) */}
            <Route path="/dashboard" element={<DashboardPage />} />
            <Route path="/patients/:patientId" element={<PatientProfilePage />} />

            {/* ✅ Guard delegates to service via ProtectedRoute (tsr-routing-003) */}
            {/* ✅ No inline role check in JSX (tsr-routing-004) */}
            <Route
                path="/admin"
                element={
                    <ProtectedRoute
                        requiredPermission="admin.access">
                        <AdminPage />
                    </ProtectedRoute>
                }
            />

            <Route path="*" element={<Navigate to="/dashboard" replace />} />
        </Routes>
    );
}
