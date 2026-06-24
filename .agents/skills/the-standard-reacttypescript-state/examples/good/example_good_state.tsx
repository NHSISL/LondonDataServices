// ---
// skill: the-standard-reacttypescript-state
// type: example
// source-section: "10. State Management"
// demonstrates: "tsr-state-001, tsr-state-002, tsr-state-004, tsr-state-007"
// ---

import { useState } from "react";
import { PatientProfileView } from "../../models/views/patients/PatientProfileView";
import { IPatientViewService } from "../../services/views/patients/iPatientViewService";

// ✅ Page hook owns server data — child components receive via props (tsr-state-002)
export function usePatientPage(
    patientId: string,
    patientViewService: IPatientViewService) {

    const [patient, setPatient] =
        useState<PatientProfileView | null>(null);

    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [error, setError] = useState<unknown>(null);

    // ✅ Business transition goes through service — not direct state mutation (tsr-state-007)
    async function handlePublishAsync() {
        try {
            const published =
                await patientViewService.publishPatientAsync(patientId);

            setPatient(published);
        } catch (caughtError: unknown) {
            setError(caughtError);
        }
    }

    return { patient, isLoading, error, handlePublishAsync };
}

// ✅ Local UI state — modal open/closed lives inside this component only (tsr-state-004)
export function PatientActionsMenu() {
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false);

    return (
        <div>
            <button type="button" onClick={() => setIsMenuOpen(!isMenuOpen)}>
                Actions
            </button>
            {isMenuOpen && <ul><li>Edit</li><li>Archive</li></ul>}
        </div>
    );
}
