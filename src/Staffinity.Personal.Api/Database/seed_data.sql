-- ============================================================
-- SEED DATA
-- ============================================================

-- 1. Genders
INSERT INTO
    public.genders (name)
VALUES ('Male'),
    ('Female'),
    ('Non-Binary'),
    ('Other'),
    ('Prefer not to say')
ON CONFLICT (name) DO NOTHING;

-- 2. Identification Types
INSERT INTO
    public.identification_types (name)
VALUES ('Citizenship ID'),
    ('Foreigner ID'),
    ('Passport'),
    ('Identity Card')
ON CONFLICT (name) DO NOTHING;

-- 3. Headquarters
INSERT INTO
    public.headquarters (name, address)
VALUES (
        'Bogotá Main HQ',
        'Calle 100 # 15-20, Bogotá'
    ),
    (
        'North HQ',
        'Carrera 7 # 120-45, Bogotá'
    ),
    (
        'Medellín HQ',
        'Calle 10 # 40-50, Medellín'
    ),
    (
        'Cali HQ',
        'Avenida 6N # 20-30, Cali'
    ),
    ('Remote HQ', 'N/A')
ON CONFLICT (name) DO NOTHING;

-- 4. Access Levels
INSERT INTO
    public.access_levels (name, description)
VALUES ('Admin', 'Full system access'),
    (
        'HR',
        'Access to reports and team management'
    ),
    (
        'Employee',
        'Basic access to employee functions'
    ),
    (
        'Contractor',
        'Limited access by contract'
    ),
    (
        'Candidate',
        'Limited read-only access'
    )
ON CONFLICT (name) DO NOTHING;

-- 5. Employee Statuses
INSERT INTO
    public.employee_statuses (name)
VALUES ('Active'),
    ('Inactive'),
    ('Suspended'),
    ('On Leave'),
    ('Retired')
ON CONFLICT (name) DO NOTHING;

-- 6. Employees (Subset of 50 for development)
INSERT INTO
    public.employees (
        employee_id,
        first_name,
        middle_name,
        last_name,
        second_last_name,
        email,
        phone_number,
        identification_number,
        date_of_birth,
        hire_date,
        gender_id,
        identification_type_id,
        headquarters_id,
        access_level_id,
        status_id,
        manager_id,
        employee_code
    )
VALUES (
        '3a824e73-ac35-4fcd-99c8-6c7963e4ba4c',
        'Carlos',
        NULL,
        'Rodriguez',
        NULL,
        'carlos.rodriguez@hexalink.com',
        '3001547294',
        '82255315',
        '1997-03-05',
        '2020-08-25',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10000'
    ),
    (
        'bb68c1fb-fd0c-4c1d-a1d4-7cddea66a25e',
        'Ana',
        NULL,
        'Martinez',
        NULL,
        'ana.martinez@hexalink.com',
        '3007741144',
        '82876498',
        '1977-06-25',
        '2020-05-03',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10001'
    ),
    (
        '2b0e0cc6-43e6-4e33-aec9-638f61c9cae4',
        'Juan',
        NULL,
        'Perez',
        NULL,
        'juan.perez@hexalink.com',
        '3004126488',
        '32124569',
        '1984-09-19',
        '2023-12-01',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Admin'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10002'
    ),
    (
        '8ab48564-ed6a-4866-bc75-64275052c44c',
        'Maria',
        NULL,
        'Lopez',
        NULL,
        'maria.lopez@hexalink.com',
        '3005163159',
        '82899371',
        '1987-01-07',
        '2022-07-26',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'HR'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10003'
    ),
    (
        '4b60acfd-dcbc-4de3-a6b3-d4e8a8564616',
        'Pedro',
        NULL,
        'Sanchez',
        NULL,
        'pedro.sanchez@hexalink.com',
        '3006913549',
        '11964115',
        '1998-02-03',
        '2021-10-30',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10004'
    ),
    (
        'f6c85151-899e-4487-902e-e2e81d9d0cfd',
        'Laura',
        NULL,
        'Gomez',
        NULL,
        'laura.gomez@hexalink.com',
        '3008820685',
        '57786745',
        '1988-12-25',
        '2024-10-13',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Medellín HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10005'
    ),
    (
        'b5440d7a-b818-428b-8049-001fd75e1f69',
        'Jorge',
        NULL,
        'Herrera',
        NULL,
        'jorge.herrera@hexalink.com',
        '3004391821',
        '50279821',
        '1989-01-27',
        '2024-11-23',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Medellín HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10006'
    ),
    (
        'efc95de0-845d-4b67-8683-c3c132fd5bff',
        'Sofia',
        NULL,
        'Ramirez',
        NULL,
        'sofia.ramirez@hexalink.com',
        '3007070335',
        '74775102',
        '1972-03-20',
        '2020-05-29',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Cali HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10007'
    ),
    (
        '61f02dd6-b98b-42d0-a1da-90d414575aad',
        'Diego',
        NULL,
        'Torres',
        NULL,
        'diego.torres@hexalink.com',
        '3006569341',
        '56723021',
        '1978-04-28',
        '2024-09-07',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'HR'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10008'
    ),
    (
        '966a8046-23bf-488f-aa31-5ce0a5af3ac1',
        'Valentina',
        NULL,
        'Morales',
        NULL,
        'valentina.morales@hexalink.com',
        '3008130650',
        '71676196',
        '1994-02-22',
        '2021-07-14',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10009'
    ),
    (
        '42e39dd1-06a3-4848-bc97-f808b76f5cb5',
        'Victoria',
        NULL,
        'Martinez',
        NULL,
        'victoria.martinez0@hexalink.com',
        '3007663312',
        '53272164',
        '1978-11-16',
        '2022-04-07',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'HR'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10010'
    ),
    (
        'ae59061a-5ac3-477a-aa7a-5d019d525221',
        'Richard',
        NULL,
        'Hernandez',
        NULL,
        'richard.hernandez1@hexalink.com',
        '3002308789',
        '85094522',
        '1997-03-14',
        '2022-06-30',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10011'
    ),
    (
        'd69b9ab9-8b08-481d-8c4d-5fd61ff53b51',
        'Daniela',
        NULL,
        'Williams',
        NULL,
        'daniela.williams2@hexalink.com',
        '3007572160',
        '54079465',
        '1984-06-20',
        '2021-09-25',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'North HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'HR'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10012'
    ),
    (
        'bee3a61a-9f0b-469e-acdf-19df15b47b1c',
        'Sofia',
        NULL,
        'Garcia',
        NULL,
        'sofia.garcia3@hexalink.com',
        '3003129525',
        '31863252',
        '1988-09-01',
        '2024-08-25',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Cali HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10013'
    ),
    (
        '04ebbfb7-db73-4c1c-890d-ee78c5ad7bf0',
        'Robert',
        NULL,
        'Sanchez',
        NULL,
        'robert.sanchez4@hexalink.com',
        '3002041980',
        '27519468',
        '1997-04-18',
        '2021-11-05',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'HR'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10014'
    ),
    (
        'f741c042-05dc-425c-9c26-661d9a2c6174',
        'Victoria',
        NULL,
        'Miller',
        NULL,
        'victoria.miller5@hexalink.com',
        '3008830139',
        '26411720',
        '1994-03-16',
        '2023-10-09',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'North HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10015'
    ),
    (
        'bf508636-10e2-4986-8118-c3da68caa0eb',
        'Camila',
        NULL,
        'Jones',
        NULL,
        'camila.jones6@hexalink.com',
        '3009093089',
        '56586541',
        '1994-06-02',
        '2022-12-27',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Medellín HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10016'
    ),
    (
        '7309d852-1c71-4f1f-8faa-b60612eb23db',
        'James',
        NULL,
        'Rodriguez',
        NULL,
        'james.rodriguez7@hexalink.com',
        '3009910701',
        '15415008',
        '1999-03-09',
        '2024-04-14',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10017'
    ),
    (
        '4f53b12b-fe1a-4dce-8c38-fa8372f5dffe',
        'David',
        NULL,
        'Torres',
        NULL,
        'david.torres8@hexalink.com',
        '3001348746',
        '80955950',
        '1973-10-04',
        '2022-10-23',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Medellín HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10018'
    ),
    (
        'e3a20d1a-633c-41b1-aa5c-687fca1ea849',
        'Daniel',
        NULL,
        'Hernandez',
        NULL,
        'daniel.hernandez9@hexalink.com',
        '3007706956',
        '57085535',
        '1989-06-08',
        '2021-10-29',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'North HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10019'
    ),
    (
        '9895f032-5c21-4b19-959b-ca9133c0045e',
        'Camila',
        NULL,
        'Brown',
        NULL,
        'camila.brown10@hexalink.com',
        '3008524460',
        '26584369',
        '1974-09-27',
        '2021-01-20',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10020'
    ),
    (
        '18bb5752-4f65-4a80-92ac-90fc0defc14f',
        'Elizabeth',
        NULL,
        'Williams',
        NULL,
        'elizabeth.williams11@hexalink.com',
        '3001652942',
        '83979018',
        '1982-08-04',
        '2023-08-07',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Medellín HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10021'
    ),
    (
        '4eb18d84-c2f9-4705-90ee-df8e38e4ae1a',
        'Gabriela',
        NULL,
        'Johnson',
        NULL,
        'gabriela.johnson12@hexalink.com',
        '3005454583',
        '35351544',
        '1998-09-21',
        '2022-02-22',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10022'
    ),
    (
        'd72316f5-21b5-44fc-bff2-34fd957131c2',
        'Felipe',
        NULL,
        'Williams',
        NULL,
        'felipe.williams13@hexalink.com',
        '3007920466',
        '72151457',
        '1996-04-11',
        '2022-06-09',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10023'
    ),
    (
        '5cd15a13-3854-40fe-b029-939db99a3481',
        'William',
        NULL,
        'Miller',
        NULL,
        'william.miller14@hexalink.com',
        '3004201876',
        '15539236',
        '1972-04-08',
        '2023-10-06',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10024'
    ),
    (
        '70c3e444-892e-4209-8a46-2fb9205e1521',
        'Alejandro',
        NULL,
        'Jones',
        NULL,
        'alejandro.jones15@hexalink.com',
        '3005073681',
        '44137558',
        '1984-07-26',
        '2022-04-29',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10025'
    ),
    (
        'a35edab8-80ea-47a4-a2ab-4ad33d448f99',
        'Barbara',
        NULL,
        'Lopez',
        NULL,
        'barbara.lopez16@hexalink.com',
        '3005979679',
        '79833378',
        '1989-08-19',
        '2023-04-29',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Medellín HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10026'
    ),
    (
        'cd10c9b4-e685-4fdd-a6fd-eeac8254d089',
        'Daniel',
        NULL,
        'Brown',
        NULL,
        'daniel.brown17@hexalink.com',
        '3007509351',
        '20162533',
        '1989-03-15',
        '2022-12-17',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10027'
    ),
    (
        '0dde516a-e582-4e79-a261-5830a28b4456',
        'Michael',
        NULL,
        'Hernandez',
        NULL,
        'michael.hernandez18@hexalink.com',
        '3001930252',
        '51216750',
        '1999-08-29',
        '2023-09-08',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10028'
    ),
    (
        '22fcb604-d993-43de-92b2-f35ea9634385',
        'Susan',
        NULL,
        'Garcia',
        NULL,
        'susan.garcia19@hexalink.com',
        '3009566542',
        '53175303',
        '1975-07-17',
        '2021-08-10',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10029'
    ),
    (
        '46bcef6c-9f49-4f22-89aa-1f755b57dea4',
        'William',
        NULL,
        'Williams',
        NULL,
        'william.williams20@hexalink.com',
        '3003181525',
        '94707103',
        '1989-05-08',
        '2023-12-05',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'North HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10030'
    ),
    (
        '803d4a09-488b-4f79-8163-60076e291e7a',
        'John',
        NULL,
        'Torres',
        NULL,
        'john.torres21@hexalink.com',
        '3004982880',
        '49578915',
        '1996-05-29',
        '2021-09-04',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'North HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10031'
    ),
    (
        'ac7c8a30-1f54-44fd-a251-a17dddaba75c',
        'Richard',
        NULL,
        'Johnson',
        NULL,
        'richard.johnson22@hexalink.com',
        '3005526457',
        '93591438',
        '1991-10-24',
        '2021-02-16',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10032'
    ),
    (
        '3a8febc3-0af3-43a4-9c0c-c0e4fd5b168d',
        'Susan',
        NULL,
        'Ramirez',
        NULL,
        'susan.ramirez23@hexalink.com',
        '3007438409',
        '54817616',
        '1995-09-09',
        '2020-06-02',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10033'
    ),
    (
        '1de663a2-2b10-411c-9232-99004ca7edf8',
        'Robert',
        NULL,
        'Johnson',
        NULL,
        'robert.johnson24@hexalink.com',
        '3009449993',
        '32878124',
        '1984-10-20',
        '2020-12-07',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Cali HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10034'
    ),
    (
        '7955fa69-9edc-40bf-ad38-09956bb6cfd4',
        'Linda',
        NULL,
        'Hernandez',
        NULL,
        'linda.hernandez25@hexalink.com',
        '3005063658',
        '36382619',
        '1996-03-26',
        '2021-07-11',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10035'
    ),
    (
        '41435551-a36a-4d3b-8a4f-5040da653a49',
        'Mateo',
        NULL,
        'Williams',
        NULL,
        'mateo.williams26@hexalink.com',
        '3005928062',
        '93878288',
        '1984-07-02',
        '2024-10-30',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'North HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10036'
    ),
    (
        'a6f0f025-d6d3-4dad-a360-c7ff095eda11',
        'Victoria',
        NULL,
        'Johnson',
        NULL,
        'victoria.johnson27@hexalink.com',
        '3006167034',
        '55472556',
        '1976-02-02',
        '2023-05-16',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'North HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10037'
    ),
    (
        'f26a5611-432c-4263-bcd8-c454ae0b5c79',
        'David',
        NULL,
        'Williams',
        NULL,
        'david.williams28@hexalink.com',
        '3005212739',
        '79360519',
        '1979-06-20',
        '2021-06-30',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10038'
    ),
    (
        'b2c4454f-9d5b-414f-92c2-99388a7c9a7c',
        'Mateo',
        NULL,
        'Lopez',
        NULL,
        'mateo.lopez29@hexalink.com',
        '3005333533',
        '69782306',
        '1977-07-14',
        '2022-02-08',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Cali HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10039'
    ),
    (
        '9c8e8c0f-e526-42bd-b8f2-80f17eee07b2',
        'Santiago',
        NULL,
        'Johnson',
        NULL,
        'santiago.johnson30@hexalink.com',
        '3008376023',
        '45586818',
        '1974-02-24',
        '2020-09-02',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Medellín HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10040'
    ),
    (
        '50f361d7-ec04-4367-9cbf-ba2ac1d82c9c',
        'Linda',
        NULL,
        'Torres',
        NULL,
        'linda.torres31@hexalink.com',
        '3005868628',
        '62324258',
        '1998-03-01',
        '2024-04-21',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Medellín HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10041'
    ),
    (
        '453213f7-1a7f-464b-8104-2b96e6d67a39',
        'Lucas',
        NULL,
        'Brown',
        NULL,
        'lucas.brown32@hexalink.com',
        '3002676479',
        '53815752',
        '1980-07-31',
        '2024-02-16',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Bogotá Main HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10042'
    ),
    (
        '9f06fef4-c8f2-41ca-ae29-023f6133c188',
        'Valentina',
        NULL,
        'Johnson',
        NULL,
        'valentina.johnson33@hexalink.com',
        '3006976556',
        '95714461',
        '1988-12-24',
        '2023-12-05',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Cali HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10043'
    ),
    (
        '517c54d0-11e6-450f-92ae-12782859d481',
        'Barbara',
        NULL,
        'Williams',
        NULL,
        'barbara.williams34@hexalink.com',
        '3007979070',
        '82520488',
        '1995-04-15',
        '2024-12-15',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10044'
    ),
    (
        '779412db-d4ac-4877-9569-8b0997eebed1',
        'Michael',
        NULL,
        'Ramirez',
        NULL,
        'michael.ramirez35@hexalink.com',
        '3003382522',
        '83488510',
        '1973-08-21',
        '2021-08-29',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10045'
    ),
    (
        '7a88f361-ead6-4f01-959a-b75974113f67',
        'Robert',
        NULL,
        'Gonzalez',
        NULL,
        'robert.gonzalez36@hexalink.com',
        '3007744698',
        '54253932',
        '1976-01-16',
        '2023-09-05',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'North HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10046'
    ),
    (
        '5d8f7338-f23b-41d1-8c9e-6673cfd79834',
        'Gabriela',
        NULL,
        'Gonzalez',
        NULL,
        'gabriela.gonzalez37@hexalink.com',
        '3002053261',
        '96180599',
        '1983-11-06',
        '2021-04-18',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Female'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Remote HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10047'
    ),
    (
        '12fe531c-ca94-4beb-bece-d66bee2468d8',
        'James',
        NULL,
        'Sanchez',
        NULL,
        'james.sanchez38@hexalink.com',
        '3002020658',
        '96004682',
        '1976-08-12',
        '2023-09-21',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Cali HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10048'
    ),
    (
        '631867f1-9ab3-419e-a31b-e69c5dda184e',
        'Daniel',
        NULL,
        'Sanchez',
        NULL,
        'daniel.sanchez39@hexalink.com',
        '3005699417',
        '25807178',
        '1995-08-24',
        '2022-11-22',
        (
            SELECT gender_id
            FROM public.genders
            WHERE
                name = 'Male'
        ),
        (
            SELECT identification_type_id
            FROM public.identification_types
            WHERE
                name = 'Citizenship ID'
        ),
        (
            SELECT headquarters_id
            FROM public.headquarters
            WHERE
                name = 'Medellín HQ'
        ),
        (
            SELECT access_level_id
            FROM public.access_levels
            WHERE
                name = 'Employee'
        ),
        (
            SELECT employee_status_id
            FROM public.employee_statuses
            WHERE
                name = 'Active'
        ),
        NULL,
        'EMP10049'
    )
ON CONFLICT (employee_id) DO NOTHING;