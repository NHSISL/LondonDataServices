// ---
// skill: the-standard-reacttypescript-testing
// type: example
// source-section: "16. Testing — anti-patterns"
// demonstrates: "tsr-testing-006, tsr-testing-007"
// ---

// ❌ BAD: Testing React internals and large snapshot as sole proof.

import { render } from "@testing-library/react";
import { describe, it, expect } from "vitest";
import { PatientProfileComponent } from "../../components/patients/PatientProfileComponent";

describe("PatientProfileComponent - BAD tests", () => {

    // ❌ Large snapshot as sole proof — violates tsr-testing-007
    it("should match snapshot", () => {
        const { container } = render(
            <PatientProfileComponent
                patientProfile={{
                    id: "abc",
                    displayName: "Elbek Alimov",
                    ageGroupDisplayText: "Adult"
                }}
            />
        );

        // Any layout tweak breaks this test without explaining what is wrong
        expect(container).toMatchSnapshot();
    });

    // ❌ Asserting on implementation detail (CSS class) — violates tsr-testing-006
    it("should have card class", () => {
        const { container } = render(
            <PatientProfileComponent
                patientProfile={{
                    id: "abc",
                    displayName: "Elbek Alimov",
                    ageGroupDisplayText: "Adult"
                }}
            />
        );

        // Tests CSS class — not visible behavior
        expect(container.firstChild).toHaveClass("patient-card");
    });
});
