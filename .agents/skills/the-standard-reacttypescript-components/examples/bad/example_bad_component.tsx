// ---
// skill: the-standard-reacttypescript-components
// type: example
// source-section: "7. Components"
// demonstrates: "tsr-components-003, tsr-components-005, tsr-components-009"
// ---

// ❌ BAD: Multiple component violations.

import { useEffect, useState } from "react";
import { Patient } from "../../models/foundations/patients/Patient";

export function PatientCard() {

    const [patient, setPatient] = useState<Patient | null>(null);

    // ❌ Broker call inside component — violates tsr-components-003
    useEffect(() => {
        fetch("/api/patients/123")
            .then(response => response.json())
            .then(setPatient);
    }, []);

    if (!patient) {
        return <div>Loading...</div>;
    }

    return (
        <div>
            <h2>{patient.firstName} {patient.lastName}</h2>

            {/* ❌ Business rule in JSX — violates tsr-components-005 */}
            <span>{patient.age >= 18 ? "Adult" : "Child"}</span>

            {/* ❌ div used as interactive control — violates tsr-components-009 */}
            <div onClick={() => console.log("save")}>Save</div>
        </div>
    );
}
