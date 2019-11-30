#pragma once

class Settings
{
public:
    Settings();
    ~Settings();

    void Load();
    void Save() const;

    bool Enabled() const
    {
        return mEnabled;
    }
    void Enabled(bool value)
    {
        mEnabled = value;
    }
    unsigned int InitialDelay() const
    {
        return mInitialDelay;
    }
    void InitialDelay(unsigned int value)
    {
        mInitialDelay = value;
    }
    unsigned int IntervalDelay() const
    {
        return mIntervalDelay;
    }
    void IntervalDelay(unsigned int value)
    {
        mIntervalDelay = value;
    }
    bool SupressFullscreen() const
    {
        return mSupressFullscreen;
    }
    void SupressFullscreen(bool value)
    {
        mSupressFullscreen = value;
    }

	const wchar_t* HyperwaveDirectory() const
    {
        return mHyperwaveDirectory;
    }

private:
    bool mEnabled;
    unsigned int mInitialDelay;
    unsigned int mIntervalDelay;
    bool mSupressFullscreen;
    wchar_t* mHyperwaveDirectory;
};
