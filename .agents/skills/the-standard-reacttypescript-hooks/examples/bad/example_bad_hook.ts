// ---
// skill: the-standard-reacttypescript-hooks
// type: example
// source-section: "9. Hooks"
// demonstrates: "tsr-hooks-002, tsr-hooks-003, tsr-hooks-005"
// ---

// ❌ BAD: Hook replaces service, contains business rules, no stale update protection.

import { useEffect, useState } from "react";

export function usePatients() {

    const [patients, setPatients] = useState<unknown[]>([]);

    useEffect(() => {
        async function load() {
            // ❌ Direct fetch — hook acting as broker — violates tsr-hooks-002
            const response = await fetch("/api/patients");
            const data = await response.json();

            // ❌ Business rule in hook — violates tsr-hooks-003
            const adults = data.filter((p: any) => p.age >= 18);

            // ❌ No mounted flag — stale update risk — violates tsr-hooks-005
            setPatients(adults);
        }

        load();
        // ❌ No cleanup function returned
    }, []);

    return { patients };
}
