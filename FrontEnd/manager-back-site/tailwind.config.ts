import type { Config } from "tailwindcss";
import colors from "tailwindcss/colors";

const config: Config = {
  content: ["./index.html", "./src/**/*.{vue,js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        brand: {
          100: colors.teal[600],
          101: colors.teal[200],
          102: colors.teal[50],
          200: colors.sky[950],
          201: colors.slate[400],
        },
        functional: {
          success: "#009626",
          error: "#CA2626",
        },
      },
    },
  },
  plugins: [],
};

export default config;
