// ---
// skill: the-standard-reacttypescript-files
// type: example
// source-section: "1. Project Structure — File Rules"
// demonstrates: "tsr-files-001, tsr-files-002, tsr-files-003, tsr-files-004"
// ---

// ✅ GOOD file naming — each file has one role, correct extension, descriptive name.

// src/brokers/apis/patientApiBroker.ts       — broker (.ts, role in name)
// src/brokers/apis/iPatientApiBroker.ts      — broker interface (.ts, i-prefix)
// src/services/foundations/patients/
//   patientService.ts                        — foundation service (.ts)
//   iPatientService.ts                       — service interface (.ts)
//   patientService.validations.ts            — validation partial (.ts)
//   patientService.exceptions.ts             — exception partial (.ts)
// src/services/views/dashboard/
//   dashboardViewService.ts                  — view service (.ts)
//   iDashboardViewService.ts                 — view service interface (.ts)
// src/pages/dashboard/
//   DashboardPage.tsx                        — page (.tsx, PascalCase, Page suffix)
//   useDashboardPage.ts                      — page hook (.ts, use-prefix)
// src/components/patients/
//   PatientSummaryCard.tsx                   — component (.tsx, PascalCase, Purpose in name)
// src/models/foundations/patients/
//   Patient.ts                               — foundation model (.ts)
// src/models/views/dashboard/
//   DashboardView.ts                         — view model (.ts)
