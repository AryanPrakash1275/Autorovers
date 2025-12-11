// src/public/Logo.tsx
import type React from "react";

type LogoProps = {
  size?: "sm" | "md" | "lg";
};

export const Logo: React.FC<LogoProps> = ({ size = "md" }) => {
  return (
    <div className={`logo logo--${size}`} aria-label="Autorovers logo">
      <span className="logo-main">Auto</span>
      <span className="logo-accent">rovers</span>
    </div>
  );
};
