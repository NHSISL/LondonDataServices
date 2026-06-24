// ---
// skill: the-standard-reacttypescript-view-services
// type: example
// source-section: "6. View Services"
// demonstrates: "tsr-viewservices-003, tsr-viewservices-004, tsr-viewservices-006"
// ---

// ❌ BAD: Multiple view service violations.

import { useCallback } from "react"; // ❌ React import — violates tsr-viewservices-003
import { PatientApiBroker } from "../../../brokers/apis/patientApiBroker"; // ❌ direct broker — violates tsr-viewservices-004

export class DashboardViewService {

    private readonly broker = new PatientApiBroker(); // ❌ direct broker — violates tsr-viewservices-004

    public async retrieveDashboardViewAsync() {

        // ❌ Calling broker directly — violates tsr-viewservices-004
        const patients = await this.broker.getPatientsAsync();

        return {
            totalPatients: patients.length,
            // ❌ CSS class name in view model — violates tsr-viewservices-006
            statusClass: patients.length > 0 ? "text-success" : "text-danger"
        };
    }
}
