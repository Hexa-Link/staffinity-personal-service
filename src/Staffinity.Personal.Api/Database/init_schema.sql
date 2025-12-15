-- ------------------------------------------------------------
-- EXTENSIONS
-- ------------------------------------------------------------
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- ------------------------------------------------------------
-- FUNCTION: update_timestamp
-- Automatically manages created_at and updated_at fields
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION public.update_timestamp()
RETURNS trigger AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        NEW.created_at = CURRENT_TIMESTAMP;
        NEW.updated_at = NULL;
    ELSIF TG_OP = 'UPDATE' THEN
        NEW.updated_at = CURRENT_TIMESTAMP;
        NEW.created_at = OLD.created_at;
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- ============================================================
-- DICTIONARY TABLES (Dependencies for Employees)
-- ============================================================

-- ------------------------------------------------------------
-- Table: genders
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS public.genders (
    gender_id UUID PRIMARY KEY DEFAULT uuid_generate_v4 (),
    name TEXT NOT NULL UNIQUE,
    created_at TIMESTAMP,
    updated_at TIMESTAMP
);

DO $$ BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_trigger WHERE tgname = 'set_genders_timestamp') THEN
        CREATE TRIGGER set_genders_timestamp
        BEFORE INSERT OR UPDATE ON public.genders
        FOR EACH ROW
        EXECUTE FUNCTION public.update_timestamp();
    END IF;
END $$;

-- ------------------------------------------------------------
-- Table: identification_types
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS public.identification_types (
    identification_type_id UUID PRIMARY KEY DEFAULT uuid_generate_v4 (),
    name TEXT NOT NULL UNIQUE,
    created_at TIMESTAMP,
    updated_at TIMESTAMP
);

DO $$ BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_trigger WHERE tgname = 'set_identification_types_timestamp') THEN
        CREATE TRIGGER set_identification_types_timestamp
        BEFORE INSERT OR UPDATE ON public.identification_types
        FOR EACH ROW
        EXECUTE FUNCTION public.update_timestamp();
    END IF;
END $$;

-- ------------------------------------------------------------
-- Table: headquarters
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS public.headquarters (
    headquarters_id UUID PRIMARY KEY DEFAULT uuid_generate_v4 (),
    name TEXT NOT NULL UNIQUE,
    address TEXT,
    created_at TIMESTAMP,
    updated_at TIMESTAMP
);

DO $$ BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_trigger WHERE tgname = 'set_headquarters_timestamp') THEN
        CREATE TRIGGER set_headquarters_timestamp
        BEFORE INSERT OR UPDATE ON public.headquarters
        FOR EACH ROW
        EXECUTE FUNCTION public.update_timestamp();
    END IF;
END $$;

-- ------------------------------------------------------------
-- Table: access_levels
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS public.access_levels (
    access_level_id UUID PRIMARY KEY DEFAULT uuid_generate_v4 (),
    name TEXT NOT NULL UNIQUE,
    description text,
    created_at TIMESTAMP,
    updated_at TIMESTAMP
);

DO $$ BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_trigger WHERE tgname = 'set_access_levels_timestamp') THEN
        CREATE TRIGGER set_access_levels_timestamp
        BEFORE INSERT OR UPDATE ON public.access_levels
        FOR EACH ROW
        EXECUTE FUNCTION public.update_timestamp();
    END IF;
END $$;

-- ------------------------------------------------------------
-- Table: employee_statuses
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS public.employee_statuses (
    employee_status_id UUID PRIMARY KEY DEFAULT uuid_generate_v4 (),
    name TEXT NOT NULL UNIQUE,
    created_at TIMESTAMP,
    updated_at TIMESTAMP
);

DO $$ BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_trigger WHERE tgname = 'set_employee_statuses_timestamp') THEN
        CREATE TRIGGER set_employee_statuses_timestamp
        BEFORE INSERT OR UPDATE ON public.employee_statuses
        FOR EACH ROW
        EXECUTE FUNCTION public.update_timestamp();
    END IF;
END $$;

-- ============================================================
-- MAIN ENTITY TABLES
-- ============================================================

-- ------------------------------------------------------------
-- Table: employees
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS public.employees (
    employee_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

-- Personal information
employee_code VARCHAR(20) NOT NULL UNIQUE,
first_name VARCHAR(40) NOT NULL,
middle_name VARCHAR(40),
last_name VARCHAR(40) NOT NULL,
second_last_name VARCHAR(40),
email VARCHAR(60) NOT NULL UNIQUE,
password_hash VARCHAR(80) NOT NULL,
phone_number VARCHAR(20),
identification_number VARCHAR(50) NOT NULL,

-- Dates (Using DATE as configured in EF)
date_of_birth DATE NOT NULL, hire_date DATE NOT NULL,

-- Relationships
-- Note: Using NO ACTION or SET NULL based on typical requirements, mimicking reference where possible
gender_id UUID NOT NULL REFERENCES public.genders (gender_id),
identification_type_id UUID NOT NULL REFERENCES public.identification_types (identification_type_id),
headquarters_id UUID NOT NULL REFERENCES public.headquarters (headquarters_id),
access_level_id UUID NOT NULL REFERENCES public.access_levels (access_level_id),
status_id UUID NOT NULL REFERENCES public.employee_statuses (employee_status_id),
manager_id UUID REFERENCES public.employees (employee_id),

-- Audit fields
is_deleted bool DEFAULT false,
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);

DO $$ BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_trigger WHERE tgname = 'set_employees_timestamp') THEN
        CREATE TRIGGER set_employees_timestamp
        BEFORE INSERT OR UPDATE ON public.employees
        FOR EACH ROW
        EXECUTE FUNCTION public.update_timestamp();
    END IF;
END $$;

-- ------------------------------------------------------------
-- Table: notifications
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS public.notifications (
    notification_id UUID PRIMARY KEY DEFAULT uuid_generate_v4 (),
    recipient_id UUID NOT NULL REFERENCES public.employees (employee_id) ON DELETE CASCADE,
    title VARCHAR(255) NOT NULL,
    message VARCHAR(255) NOT NULL,
    is_read BOOLEAN NOT NULL DEFAULT FALSE,
    related_url VARCHAR(255),
    sent_date TIMESTAMP DEFAULT NOW(),
    created_at TIMESTAMP,
    updated_at TIMESTAMP
);

DO $$ BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_trigger WHERE tgname = 'set_notifications_timestamp') THEN
        CREATE TRIGGER set_notifications_timestamp
        BEFORE INSERT OR UPDATE ON public.notifications
        FOR EACH ROW
        EXECUTE FUNCTION public.update_timestamp();
    END IF;
END $$;

-- ------------------------------------------------------------
-- Table: vacation_requests
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS public.vacation_requests (
    vacation_request_id UUID PRIMARY KEY DEFAULT uuid_generate_v4 (),
    employee_id UUID NOT NULL REFERENCES public.employees (employee_id) ON DELETE CASCADE,
    start_date TIMESTAMP NOT NULL,
    end_date TIMESTAMP NOT NULL,
    reason VARCHAR(500),
    status TEXT NOT NULL,
    created_at TIMESTAMP,
    updated_at TIMESTAMP
);

DO $$ BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_trigger WHERE tgname = 'set_vacation_requests_timestamp') THEN
        CREATE TRIGGER set_vacation_requests_timestamp
        BEFORE INSERT OR UPDATE ON public.vacation_requests
        FOR EACH ROW
        EXECUTE FUNCTION public.update_timestamp();
    END IF;
END $$;