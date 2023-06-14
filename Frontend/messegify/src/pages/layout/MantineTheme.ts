import {CSSObject, MantineColor, MantineThemeBase, Tuple} from "@mantine/core";
import {CSSProperties} from "react";

interface MantineTheme {
    // Defines color scheme for all components, defaults to "light"
    colorScheme: 'light' | 'dark';

    // Controls focus ring styles:
    // auto – display focus ring only when user navigates with keyboard (default)
    // always – display focus ring when user navigates with keyboard and mouse
    // never – focus ring is always hidden (not recommended)
    focusRing: 'auto' | 'always' | 'never';

    // Change focus ring styles
    focusRingStyles: {
        styles(theme: MantineThemeBase): CSSObject;
        resetStyles(theme: MantineThemeBase): CSSObject;
        inputStyles(theme: MantineThemeBase): CSSObject;
    };

    // Determines whether motion based animations should be disabled for
    // users who prefer to reduce motion in their OS settings
    respectReducedMotion: boolean;

    // Determines whether elements that do not have pointer cursor by default
    // (checkboxes, radio, native select) should have it
    cursorType: 'default' | 'pointer';

    // Default border-radius used for most elements
    defaultRadius: 'xs' | 'sm' | 'md' | 'lg' | 'xl' | number;

    // White and black colors, defaults to '#fff' and '#000'
    white: string;
    black: string;

    // Object of arrays with 10 colors
    colors: Record<string, Tuple<string, 10>>;

    // Key of theme.colors
    primaryColor: string;

    // Index of color from theme.colors that is considered primary, Shade type is 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9
    primaryShade: 0 | { light: 1; dark: 2 };

    // Default gradient used in components that support `variant="gradient"` (Button, ThemeIcon, etc.)
    defaultGradient: { deg: number; from: MantineColor; to: MantineColor };

    // font-family and line-height used in most components
    fontFamily: string;
    lineHeight: string | number;

    // Timing function used for animations, defaults to 'ease'
    transitionTimingFunction: string;

    // Monospace font-family, used in Code, Kbd and Prism components
    fontFamilyMonospace: string;

    // Sizes for corresponding properties
    fontSizes: Record<'xs' | 'sm' | 'md' | 'lg' | 'xl', number>;
    radius: Record<'xs' | 'sm' | 'md' | 'lg' | 'xl', number>;
    spacing: Record<'xs' | 'sm' | 'md' | 'lg' | 'xl', number>;

    // Values used for box-shadow
    shadows: Record<'xs' | 'sm' | 'md' | 'lg' | 'xl', string>;

    // Breakpoints used in some components to add responsive styles
    breakpoints: Record<'xs' | 'sm' | 'md' | 'lg' | 'xl', number>;

    // Styles added to buttons with `:active` pseudo-class
    activeStyles: CSSObject;

    // h1-h6 styles, used in Title and TypographyStylesProvider components
    headings: {
        fontFamily: CSSProperties['fontFamily'];
        fontWeight: CSSProperties['fontWeight'];
        sizes: {
            // See heading options below
            h1: Heading;
            h2: Heading;
            h3: Heading;
            h4: Heading;
            h5: Heading;
            h6: Heading;
        };
    };

    // theme functions, see in theme functions guide
    //fn: MantineThemeFunctions;

    // Left to right or right to left direction, see RTL Support guide to learn more
    dir: 'ltr' | 'rtl';

    // Default loader used in Loader and LoadingOverlay components
    loader: 'oval' | 'bars' | 'dots';

    // Default date format used in DatePicker and DateRangePicker components
    dateFormat: string;

    // Default dates formatting locale used in every @mantine/dates component
    datesLocale: string;

    // defaultProps, styles and classNames for components
    //components: ComponentsOverride;

    // Global styles
    globalStyles: (theme: MantineTheme) => CSSObject;

    // Add your own custom properties on Mantine theme
    other: Record<string, any>;
}

interface Heading {
    fontSize: CSSProperties['fontSize'];
    fontWeight: CSSProperties['fontWeight'];
    lineHeight: CSSProperties['lineHeight'];
}