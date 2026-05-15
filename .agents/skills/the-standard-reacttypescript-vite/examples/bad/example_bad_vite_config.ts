// ---
// skill: the-standard-reacttypescript-vite
// type: example
// source-section: "15. Vite"
// demonstrates: "tsr-vite-001, tsr-vite-003, tsr-vite-005"
// ---

import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import svgr from "vite-plugin-svgr";         // ❌ no comment explaining purpose — violates tsr-vite-005
import { visualizer } from "rollup-plugin-visualizer"; // ❌ no comment — violates tsr-vite-005
import path from "path";

export default defineConfig({
    plugins: [
        react(),
        svgr(),
        visualizer()
    ],

    resolve: {
        alias: {
            "@": path.resolve(__dirname, "./src"),
            "@brokers": path.resolve(__dirname, "./src/brokers"),
            // ❌ Per-subfolder alias — not recommended (tsr-vite-004)
            "@patientBrokers": path.resolve(__dirname, "./src/brokers/apis/patients")
        }
    },

    define: {
        // ❌ Business logic in config — feature flag evaluation at build time — violates tsr-vite-001
        __FEATURE_X_ENABLED__: process.env.FEATURE_X === "true"
    }
});

// ❌ .env file would contain:
// VITE_STRIPE_SECRET_KEY=sk_live_abc123   <-- secret exposed to client — violates tsr-vite-003
