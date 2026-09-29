import PageLayout from "../components/PageLayout.jsx";
import { SITE_NAME } from "../config.js";
import AudiencesSection from "./landing/AudiencesSection.jsx";
import CallToActionSection from "./landing/CallToActionSection.jsx";
import HeroSection from "./landing/HeroSection.jsx";
import HowItWorksSection from "./landing/HowItWorksSection.jsx";

export default function Landing() {
  return (
    <PageLayout title={SITE_NAME} showSectionLinks>
      <HeroSection />
      <HowItWorksSection />
      <AudiencesSection />
      <CallToActionSection />
    </PageLayout>
  );
}
