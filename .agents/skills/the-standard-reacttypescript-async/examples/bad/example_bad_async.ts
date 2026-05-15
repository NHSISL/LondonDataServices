// ---
// skill: the-standard-reacttypescript-async
// type: example
// source-section: "14. Async and Cancellation"
// demonstrates: "tsr-async-002, tsr-async-004, tsr-async-005"
// ---

// ❌ BAD: No stale protection, dependent parallel ops, ignored rejection.

import { useState, useEffect } from "react";

export function useBadAsync(service: any) {

    const [data, setData] = useState(null);

    useEffect(() => {
        async function loadAsync() {
            // ❌ No try/catch — rejection ignored — violates tsr-async-005
            const user = await service.getUserAsync();

            // ❌ Dependent operations run in parallel — userId not yet safe — violates tsr-async-004
            const [profile, permissions] = await Promise.all([
                service.getProfileAsync(user.id),
                service.getPermissionsAsync(user.id)
            ]);

            // ❌ No mounted flag — may set state after unmount — violates tsr-async-002
            setData({ profile, permissions });
        }

        loadAsync();
        // ❌ No cleanup function returned — violates tsr-async-002
    }, [service]);

    return { data };
}
