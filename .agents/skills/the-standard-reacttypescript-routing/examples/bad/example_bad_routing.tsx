// ---
// skill: the-standard-reacttypescript-routing
// type: example
// source-section: "11. Routing"
// demonstrates: "tsr-routing-001, tsr-routing-004"
// ---

// ❌ BAD: Route to component, auth rule in JSX.

import { Routes, Route } from "react-router-dom";
import { PatientCard } from "../components/patients/PatientCard";
import { AdminPage } from "../pages/admin/AdminPage";

export function AppRoutes() {

    const user = { role: "Admin" }; // obtained from somewhere

    return (
        <Routes>
            {/* ❌ Route points to a card component — violates tsr-routing-001 */}
            <Route path="/patients/:id" element={<PatientCard />} />

            {/* ❌ Business authorization rule in JSX — violates tsr-routing-004 */}
            {user.role === "Admin" && (
                <Route path="/admin" element={<AdminPage />} />
            )}
        </Routes>
    );
}
