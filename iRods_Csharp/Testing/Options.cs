// ReSharper disable InconsistentNaming
namespace Testing;

internal record AccountOptions(
    string irods_encryption_algorithm,
    int irods_encryption_key_size,
    int irods_encryption_salt_size,
    int irods_encryption_num_hash_rounds,
    string irods_authentication_scheme,

    string irods_host,
    int irods_port,
    string irods_home,
    string irods_zone_name,
    string irods_user_name,
    string? irods_password = null
);