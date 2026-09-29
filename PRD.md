#Product Requirement Document (PRD) - ByteBrief

**ByteBrief**
**Version:1.0**
**Authors: Árthur Alejandro**

--- 

## Description
    ByteBrief is an automated background service designed to keep you connected every day with what is new around the tech world by aggregating, summarizing, and delivering audio/text news digests directly to Telegram.

## Target audience
    Software engineers, tech enthusiasts, and busy professionals who want a quick, automated daily audio/text briefing of top tech news without having to browse multiple forums or portals.
## Scope
    ### In Scope (MVP)
        * Automated daily news scraping from pre-defined sources (Reddit, Tech Portals/RSS).
        * Filtering and deduplication based on metrics and database history.
        * Summarization of top stories into a unified daily script using an AI LLM.
        * Conversion of the summary script into an MP3 audio file via Text-to-Speech (TTS) API.
        * Automatic publication of text summary + audio file to a Telegram chat/channel.

--- 

## Functional Requirements
    * **FR001: News Scraping**
    * The system must access pre-defined news sources (to define).
    * The system must extract content, engagement metrics (upvotes, likes, views, etc), and original URLs.
    * The scraping process must run automatically every day on a fixed schedule.

    * **FR002: Data Persistence & Deduplication**
    * The system must persist all fetched article references (URLs, fetched dates, status) in the PostgreSQL database.
    * The system must check existing URLs in the database prior to processing to prevent duplicated news.

    * **FR003: Filtering & Selection**
    * The system must filter and rank collected news based on engagement metrics and publication date.
    * The system must select the top N most relevant news items (e.g., Top 4) for the daily summary.

    * **FR004: Text Summarization via AI**
    * The system must send the selected top news content to an AI API (e.g., OpenAI ChatGPT / Gemini).
    * The AI must return a cohesive, single daily briefing script formatted in Markdown.

    * **FR005: Text-to-Speech (TTS) Generation**
    * The system must send the generated briefing script to a Text-to-Speech API.
    * The API must return an `.mp3` audio file representing the vocalized news summary.

    * **FR006: Telegram Dispatcher**
    * The system must interact with the Telegram Bot API to deliver the generated summary.
    * The dispatch must include both the text summary (with source links) and the `.mp3` audio file attachment.

--- 

## Non-Functional Requirements
    * **NFR001: Security & Configuration**
    * Sensitive data (database connection strings, API keys for OpenAI/Telegram) must be loaded from environment variables or `appsettings.json` and never hardcoded in source control. 
