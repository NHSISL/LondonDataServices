// ---
// skill: the-standard-reacttypescript-view-services
// type: example
// source-section: "6. View Services"
// demonstrates: "tsr-viewservices-001, tsr-viewservices-005, tsr-viewservices-006"
// ---

import { IPatientService } from "../../foundations/patients/iPatientService";
import { IAppointmentService } from "../../foundations/appointments/iAppointmentService";
import { DashboardView } from "../../../models/views/dashboard/DashboardView";

export interface IDashboardViewService {
    retrieveDashboardViewAsync(): Promise<DashboardView>;
}

export class DashboardViewService implements IDashboardViewService {

    public constructor(
        private readonly patientService: IPatientService,
        private readonly appointmentService: IAppointmentService) {
    }

    // ✅ Coordinates foundation services, maps to view model, no CSS classes
    public async retrieveDashboardViewAsync()
        : Promise<DashboardView> {

        const [patients, appointments] =
            await Promise.all([
                this.patientService.retrievePatientsAsync(),
                this.appointmentService.retrieveUpcomingAppointmentsAsync()
            ]);

        return {
            totalPatients: patients.length,
            upcomingAppointments: appointments.length,
            // ✅ display text value — not a CSS class
            activityStatusText: patients.length > 0 ? "Active" : "No patients"
        };
    }
}
