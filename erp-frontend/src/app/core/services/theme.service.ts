import { Injectable } from '@angular/core';

export interface ThemeConfig {
  // Primary
  primaryColor: string;
  primaryDark: string;
  primaryLight: string;
  primaryGradient: string;
  secondaryColor: string;

  // Status
  successColor: string;
  successLight: string;
  warningColor: string;
  warningLight: string;
  dangerColor: string;
  dangerLight: string;
  infoColor: string;
  infoLight: string;

  // Text
  textPrimary: string;
  textSecondary: string;
  textMuted: string;

  // Backgrounds
  bgPrimary: string;
  bgSecondary: string;
  bgTertiary: string;

  // Borders
  borderColor: string;
  borderLight: string;

  // Font Sizes
  fontSizeH1: string;
  fontSizeH2: string;
  fontSizeH3: string;
  fontSizeH4: string;
  fontSizeBase: string;
  fontSizeSm: string;
  fontSizeXs: string;
  fontSizeLabel: string;
  fontSizeInput: string;
  fontSizeBtn: string;
  fontSizeBtnSm: string;

  // Icon Sizes
  iconSizeLg: string;
  iconSizeMd: string;
  iconSizeBase: string;
  iconSizeSm: string;

  // Border Radius
  radiusLg: string;
  radiusMd: string;
  radiusBase: string;
  radiusSm: string;
  radiusXs: string;

  // Button Heights
  btnHeight: string;
  btnHeightLg: string;

  // Shadows
  shadowSm: string;
  shadowMd: string;
  shadowLg: string;
  shadowCard: string;
}

const DEFAULT_THEME: ThemeConfig = {
  // Primary
  primaryColor: '#002e5c',
  primaryDark: '#002147',
  primaryLight: '#004785',
  primaryGradient: 'linear-gradient(135deg, #004785 0%, #002e5c 50%, #002147 100%)',
  secondaryColor: '#005494',

  // Status
  successColor: '#10b981',
  successLight: '#d1fae5',
  warningColor: '#f59e0b',
  warningLight: '#fef3c7',
  dangerColor: '#ef4444',
  dangerLight: '#fee2e2',
  infoColor: '#3b82f6',
  infoLight: '#dbeafe',

  // Text
  textPrimary: '#111827',
  textSecondary: '#6b7280',
  textMuted: '#9ca3af',

  // Backgrounds
  bgPrimary: '#ffffff',
  bgSecondary: '#f9fafb',
  bgTertiary: '#f3f4f6',

  // Borders
  borderColor: '#e5e7eb',
  borderLight: '#f3f4f6',

  // Font Sizes
  fontSizeH1: '1.5rem',
  fontSizeH2: '1.25rem',
  fontSizeH3: '0.9375rem',
  fontSizeH4: '0.8125rem',
  fontSizeBase: '0.8125rem',
  fontSizeSm: '0.75rem',
  fontSizeXs: '0.6875rem',
  fontSizeLabel: '0.8125rem',
  fontSizeInput: '0.8125rem',
  fontSizeBtn: '0.8125rem',
  fontSizeBtnSm: '0.75rem',

  // Icon Sizes
  iconSizeLg: '24px',
  iconSizeMd: '20px',
  iconSizeBase: '16px',
  iconSizeSm: '14px',

  // Border Radius
  radiusLg: '16px',
  radiusMd: '12px',
  radiusBase: '8px',
  radiusSm: '6px',
  radiusXs: '5px',

  // Button Heights
  btnHeight: '36px',
  btnHeightLg: '44px',

  // Shadows
  shadowSm: '0 2px 6px rgba(0, 0, 0, 0.08)',
  shadowMd: '0 4px 12px rgba(0, 0, 0, 0.1)',
  shadowLg: '0 6px 20px rgba(0, 46, 92, 0.35)',
  shadowCard: '0 2px 10px rgba(0, 46, 92, 0.08)',
};

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private currentTheme: ThemeConfig = { ...DEFAULT_THEME };

  constructor() {
    this.applyTheme(this.currentTheme);
  }

  /** Apply full theme to CSS custom properties */
  applyTheme(theme: Partial<ThemeConfig>): void {
    this.currentTheme = { ...this.currentTheme, ...theme };
    const root = document.documentElement;

    const cssVarMap: Record<string, string> = {
      // Primary
      '--primary-color': this.currentTheme.primaryColor,
      '--primary-dark': this.currentTheme.primaryDark,
      '--primary-light': this.currentTheme.primaryLight,
      '--primary-gradient': this.currentTheme.primaryGradient,
      '--secondary-color': this.currentTheme.secondaryColor,

      // Status
      '--success-color': this.currentTheme.successColor,
      '--success-light': this.currentTheme.successLight,
      '--warning-color': this.currentTheme.warningColor,
      '--warning-light': this.currentTheme.warningLight,
      '--danger-color': this.currentTheme.dangerColor,
      '--danger-light': this.currentTheme.dangerLight,
      '--info-color': this.currentTheme.infoColor,
      '--info-light': this.currentTheme.infoLight,

      // Text
      '--text-primary': this.currentTheme.textPrimary,
      '--text-secondary': this.currentTheme.textSecondary,
      '--text-muted': this.currentTheme.textMuted,

      // Backgrounds
      '--bg-primary': this.currentTheme.bgPrimary,
      '--bg-secondary': this.currentTheme.bgSecondary,
      '--bg-tertiary': this.currentTheme.bgTertiary,

      // Borders
      '--border-color': this.currentTheme.borderColor,
      '--border-light': this.currentTheme.borderLight,

      // Font Sizes
      '--font-size-h1': this.currentTheme.fontSizeH1,
      '--font-size-h2': this.currentTheme.fontSizeH2,
      '--font-size-h3': this.currentTheme.fontSizeH3,
      '--font-size-h4': this.currentTheme.fontSizeH4,
      '--font-size-base': this.currentTheme.fontSizeBase,
      '--font-size-sm': this.currentTheme.fontSizeSm,
      '--font-size-xs': this.currentTheme.fontSizeXs,
      '--font-size-label': this.currentTheme.fontSizeLabel,
      '--font-size-input': this.currentTheme.fontSizeInput,
      '--font-size-btn': this.currentTheme.fontSizeBtn,
      '--font-size-btn-sm': this.currentTheme.fontSizeBtnSm,

      // Icon Sizes
      '--icon-size-lg': this.currentTheme.iconSizeLg,
      '--icon-size-md': this.currentTheme.iconSizeMd,
      '--icon-size-base': this.currentTheme.iconSizeBase,
      '--icon-size-sm': this.currentTheme.iconSizeSm,

      // Border Radius
      '--radius-lg': this.currentTheme.radiusLg,
      '--radius-md': this.currentTheme.radiusMd,
      '--radius-base': this.currentTheme.radiusBase,
      '--radius-sm': this.currentTheme.radiusSm,
      '--radius-xs': this.currentTheme.radiusXs,

      // Button Heights
      '--btn-height': this.currentTheme.btnHeight,
      '--btn-height-lg': this.currentTheme.btnHeightLg,

      // Shadows
      '--shadow-sm': this.currentTheme.shadowSm,
      '--shadow-md': this.currentTheme.shadowMd,
      '--shadow-lg': this.currentTheme.shadowLg,
      '--shadow-card': this.currentTheme.shadowCard,
    };

    Object.entries(cssVarMap).forEach(([prop, value]) => {
      root.style.setProperty(prop, value);
    });
  }

  /** Get current theme config */
  getTheme(): ThemeConfig {
    return { ...this.currentTheme };
  }

  /** Update specific theme properties */
  updateTheme(partial: Partial<ThemeConfig>): void {
    this.applyTheme(partial);
  }
}
