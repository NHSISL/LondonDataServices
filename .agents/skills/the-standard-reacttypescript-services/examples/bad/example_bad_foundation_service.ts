// ---
// skill: the-standard-reacttypescript-services
// type: example
// source-section: "5. Foundation Services"
// demonstrates: "tsr-foundation-002, tsr-foundation-003, tsr-foundation-004, tsr-foundation-006"
// ---

// ❌ BAD: Multiple foundation service violations.

import { useState } from "react";           // ❌ React import — violates tsr-foundation-002
import { useNavigate } from "react-router-dom"; // ❌ React import — violates tsr-foundation-002

export class PatientService {

    public async retrievePatientAsync(
        patientId: string)
        : Promise<unknown> {

        // ❌ No input validation before broker call — violates tsr-foundation-004
        const response = await fetch(`/api/patients/${patientId}`);

        if (!response.ok) {
            // ❌ Navigation in service — violates tsr-foundation-006
            const navigate = useNavigate();
            navigate("/error");

            // ❌ JSX returned from service — violates tsr-foundation-003
            return <div>Error loading patient</div>;
        }

        return response.json();
    }
}
