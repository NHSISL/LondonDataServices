// ---
// skill: the-standard-reacttypescript-vite
// type: example
// source-section: "15. Vite"
// demonstrates: "tsr-vite-001, tsr-vite-004, tsr-vite-005"
// ---

import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import path from "path";

export default defineConfig({
    plugins: [
        // React fast refresh and JSX transform
        react()
    ],

    resolve: {
        alias: {
            // ✅ Consistent recommended aliases only (tsr-vite-004)
            "@": path.resolve(__dirname, "./src"),
            "@brokers": path.resolve(__dirname, "./src/brokers"),
            "@services": path.resolve(__dirname, "./src/services"),
            "@models": path.resolve(__dirname, "./src/models"),
            "@components": path.resolve(__dirname, "./src/components"),
            "@pages": path.resolve(__dirname, "./src/pages")
        }
    }
    // ✅ No business logic — infrastructure configuration only (tsr-vite-001)
});
