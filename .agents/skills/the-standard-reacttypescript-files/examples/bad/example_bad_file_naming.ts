// ---
// skill: the-standard-reacttypescript-files
// type: example
// source-section: "1. Project Structure — File Rules"
// demonstrates: "tsr-files-001, tsr-files-002, tsr-files-003, tsr-files-004, tsr-files-005"
// ---

// ❌ BAD file naming — multiple violations.

// src/utils.ts
//   ❌ generic name — violates tsr-files-005
//   ❌ mixes formatting, validation, and date helpers in one file — violates tsr-files-001

// src/helpers.ts
//   ❌ generic name — violates tsr-files-005

// src/common.ts
//   ❌ generic name — violates tsr-files-005

// src/patient.ts
//   ❌ no architectural role in name — violates tsr-files-002
//   ❌ mixes broker + service + model — violates tsr-files-001

// src/patientService.tsx
//   ❌ non-rendering service uses .tsx — violates tsr-files-004

// src/PatientCard.ts
//   ❌ React component file missing .tsx extension — violates tsr-files-003

// src/Dashboard.tsx
//   ❌ page file missing Page suffix — violates tsr-files-002
