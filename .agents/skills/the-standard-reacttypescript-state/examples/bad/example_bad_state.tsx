// ---
// skill: the-standard-reacttypescript-state
// type: example
// source-section: "10. State Management"
// demonstrates: "tsr-state-002, tsr-state-006, tsr-state-007"
// ---

// ❌ BAD: Duplicated server state, global state for prop avoidance, direct state transition.

import { createContext, useState } from "react";
import { Patient } from "../../models/foundations/patients/Patient";

// ❌ Global context created to avoid threading props — violates tsr-state-006
export const SelectedPatientContext =
    createContext<Patient | null>(null);

export function PatientSummaryCard() {
    // ❌ Child component duplicates server fetch — violates tsr-state-002
    const [patient, setPatient] = useState<Patient | null>(null);

    // Fetches the same data the parent page hook already fetched
    // useEffect(() => { fetch("/api/patients/123").then(...).then(setPatient); }, []);

    async function handlePublish() {
        if (!patient) return;

        // ❌ Business transition directly in state setter — violates tsr-state-007
        setPatient({ ...patient, status: "Published" });
        // No service call — validation and exception handling are bypassed
    }

    return (
        <div>
            <button type="button" onClick={handlePublish}>Publish</button>
        </div>
    );
}
